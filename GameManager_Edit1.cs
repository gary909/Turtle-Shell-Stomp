using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using MoreMountains.InventoryEngine;
using MoreMountains.Feedbacks;
using System;
using UnityEngine.UI;

//	V1 - Lives UI now working, using GameManager_Edit1, g_HealthBar9, LivesManager6

namespace MoreMountains.CorgiEngine
{
	/// <summary>
	/// A list of the possible Corgi Engine base events
	/// LevelStart : triggered by the LevelManager when a level starts
	///	LevelComplete : can be triggered when the end of a level is reached
	/// LevelEnd : same thing
	///	Pause : triggered when a pause is starting
	///	UnPause : triggered when a pause is ending and going back to normal
	///	PlayerDeath : triggered when the player character dies
	///	Respawn : triggered when the player character respawns
	///	StarPicked : triggered when a star bonus gets picked
	///	GameOver : triggered by the LevelManager when all lives are lost
	/// CharacterSwitch : triggered when the character gets switched
	/// CharacterSwap : triggered when the character gets swapped
	/// TogglePause : triggered to request a pause (or unpause)
	/// </summary>
	public enum CorgiEngineEventTypes
	{
		SpawnCharacterStarts,
		LevelStart,
		LevelComplete,
		LevelEnd,
		Pause,
		UnPause,
		PlayerDeath,
		Respawn,
		StarPicked,
		GameOver,
		CharacterSwitch,
		CharacterSwap,
		TogglePause,
		LoadNextScene,
		PauseNoMenu
	}


	/// <summary>
	/// A type of events used to signal level start and end (for now)
	/// </summary>
	public struct CorgiEngineEvent
	{
		public CorgiEngineEventTypes EventType;
		public Character OriginCharacter;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="MoreMountains.CorgiEngine.CorgiEngineEvent"/> struct.
		/// </summary>
		/// <param name="eventType">Event type.</param>
		public CorgiEngineEvent(CorgiEngineEventTypes eventType, Character originCharacter = null)
		{
			EventType = eventType;
			OriginCharacter = originCharacter;
		}
        
		static CorgiEngineEvent e;
		public static void Trigger(CorgiEngineEventTypes eventType, Character originCharacter = null)
		{
			e.EventType = eventType;
			e.OriginCharacter = originCharacter;
			MMEventManager.TriggerEvent(e);
		}
	} 

	/// <summary>
	/// A list of the methods available to change the current score
	/// </summary>
	public enum PointsMethods
	{
		Add,
		Set
	}

	//  Method to allow external scripts to subscribe to OnLivesChanged
	/*public void SubscribeToLivesChanged(Action action)
	{
		OnLivesChanged -= action; // Prevent duplicate subscriptions
		OnLivesChanged += action;
	}

	//  Method to allow external scripts to unsubscribe from OnLivesChanged
	public void UnsubscribeFromLivesChanged(Action action)
	{
		OnLivesChanged -= action;
	}*/


	public struct CorgiEngineStarEvent
	{
		public string SceneName;
		public int StarID;

		public CorgiEngineStarEvent(string sceneName, int starID)
		{
			SceneName = sceneName;
			StarID = starID;
		}

		static CorgiEngineStarEvent e;
		public static void Trigger(string sceneName, int starID)
		{
			e.SceneName = sceneName;
			e.StarID = starID;
			MMEventManager.TriggerEvent(e);
		}
	}

	/// <summary>
	/// A type of event used to signal changes to the current score
	/// </summary>
	public struct CorgiEnginePointsEvent
	{
		public PointsMethods PointsMethod;
		public int Points;
		/// <summary>
		/// Initializes a new instance of the <see cref="MoreMountains.CorgiEngine.CorgiEnginePointsEvent"/> struct.
		/// </summary>
		/// <param name="pointsMethod">Points method.</param>
		/// <param name="points">Points.</param>
		public CorgiEnginePointsEvent(PointsMethods pointsMethod, int points)
		{
			PointsMethod = pointsMethod;
			Points = points;
		}
        
		static CorgiEnginePointsEvent e;
		public static void Trigger(PointsMethods pointsMethod, int points)
		{
			e.PointsMethod = pointsMethod;
			e.Points = points;
			MMEventManager.TriggerEvent(e);
		}
	}

	public enum PauseMethods
	{
		PauseMenu,
		NoPauseMenu
	}

	public class PointsOfEntryStorage
	{
		public string LevelName;
		public int PointOfEntryIndex;
		public Character.FacingDirections FacingDirection;

		public PointsOfEntryStorage(string levelName, int pointOfEntryIndex, Character.FacingDirections facingDirection)
		{
			LevelName = levelName;
			FacingDirection = facingDirection;
			PointOfEntryIndex = pointOfEntryIndex;
		}
	}

	/// <summary>
	/// The game manager is a persistent singleton that handles points and time
	/// </summary>
	[AddComponentMenu("Corgi Engine/Managers/Game Manager")]
	public class GameManager : 	MMPersistentSingleton<GameManager>, 
		MMEventListener<MMGameEvent>, 
		MMEventListener<CorgiEngineEvent>, 
		MMEventListener<CorgiEnginePointsEvent>
	{		
		[Header("Settings")]

		/// the target frame rate for the game
		[Tooltip("the target frame rate for the game")]
		public int TargetFrameRate=300;



		[Header("Lives")]

		/// the maximum amount of lives the character can currently have
		[Tooltip("the maximum amount of lives the character can currently have")]
		public int MaximumLives = 100;

		//[Header("Lives UI Text")]
		//[Tooltip("text for lives UI")]
		//public Text LifeCountText; // Reference to UI text *******************************************************************

		//public delegate void LivesChangedDelegate(); // GJW: for updating UI lives Text
		//public event LivesChangedDelegate OnLivesChanged; // GJW: for updating UI lives Text

		public event Action OnLivesChanged = delegate { }; // Prevents NULL reference issues



		/// the current number of lives 
		//[Tooltip("the current number of lives ")]
		[System.NonSerialized] public int CurrentLives = 5; // Prevents Unity from storing its value

		[Header("Game Over")]
		/// if this is true, lives will be reset on game over
		[Tooltip("if this is true, lives will be reset on game over")]
		public bool ResetLivesOnGameOver = true;
		/// if this is true, the persistent character will be cleared on game over
		[Tooltip("if this is true, the persistent character will be cleared on game over")]
		public bool ResetPersistentCharacterOnGameOver = true;
		/// if this is true, the stored character will be cleared on game over
		[Tooltip("if this is true, the stored character will be cleared on game over")]
		public bool ResetStoredCharacterOnGameOver = true;
		/// the name of the scene to redirect to when all lives are lost
		[Tooltip("the name of the scene to redirect to when all lives are lost")]
		public string GameOverScene;

		/// the current number of game points
		public int Points { get; private set; }
		/// true if the game is currently paused
		public bool Paused { get; set; } 
		// true if we've stored a map position at least once
		public bool StoredLevelMapPosition{ get; set; }
		/// the current player
		public Vector2 LevelMapPosition { get; set; }
		/// the stored selected character
		public Character StoredCharacter { get; set; }
		/// the stored selected character
		public Character PersistentCharacter { get; set; }
		/// the list of points of entry and exit
		public List<PointsOfEntryStorage> PointsOfEntry { get; set; }

		protected bool _inventoryOpen = false;
		protected bool _pauseMenuOpen = false;
		protected InventoryInputManager _inventoryInputManager;
		protected int _initialMaximumLives;
		protected int _initialCurrentLives;

		protected override void Awake()
		{
			base.Awake ();
			PointsOfEntry = new List<PointsOfEntryStorage> ();
		}

		/// <summary>
		/// On Start(), sets the target framerate to whatever's been specified
		/// </summary>
		/*protected virtual void Start()
		{
			Application.targetFrameRate = TargetFrameRate;

			// Ensure CurrentLives is set correctly at the start of the game
			if (CurrentLives == 0) // Prevents starting with 0 lives
			{
				CurrentLives = 5; // Set default starting lives
			}

			Debug.Log("GameManager Start() - Initial Lives: " + CurrentLives);

			// Store the initial lives for reset purposes
			_initialCurrentLives = CurrentLives;
			_initialMaximumLives = MaximumLives;

			// Force UI to update at start
			OnLivesChanged?.Invoke();
		}*/

		protected virtual void Start()
		{
			Application.targetFrameRate = TargetFrameRate;

			if (OnLivesChanged == null)
			{
				OnLivesChanged = delegate { };
			}

			Debug.Log("GameManager Start() - Initial Lives: " + CurrentLives);
		}



		/// <summary>
		/// this method resets the whole game manager
		/// </summary>
		public virtual void Reset()
		{
			Points = 0;
			MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 1f, 0f, false, 0f, true);
			Paused = false;
			GUIManager.Instance.RefreshPoints ();
			PointsOfEntry?.Clear ();
		}	

		/// <summary>
		/// Use this method to decrease the current number of lives
		/// </summary>
		public virtual void LoseLife()
		{
			CurrentLives--;

			// Ensure the UI updates immediately
			OnLivesChanged?.Invoke();

			// Check if lives are at 0 and trigger game over
			if (CurrentLives <= 0)
			{
				CurrentLives = 0; // Prevent negative values
				CorgiEngineEvent.Trigger(CorgiEngineEventTypes.GameOver);
			}
		}

		/// <summary>
		/// Use this method when a life (or more) is gained
		/// </summary>
		/// <param name="lives">Lives.</param>
		/*public virtual void GainLives(int lives)
		{
			Debug.Log("GainLives called. Current Lives BEFORE: " + CurrentLives + " | Adding: " + lives);

			CurrentLives += lives;

			if (CurrentLives > MaximumLives)
			{
				CurrentLives = MaximumLives;
			}

			Debug.Log("GainLives - New Life Count: " + CurrentLives);

			// Fire the event properly so LivesCounter updates
			if (OnLivesChanged != null)
			{
				OnLivesChanged.Invoke();
				Debug.Log("OnLivesChanged event triggered successfully.");
			}
			else
			{
				Debug.LogWarning("OnLivesChanged event is NULL! UI will not update.");
			}
		}*/

		public virtual void GainLives(int lives)
		{
			Debug.Log($"GainLives called. Current Lives BEFORE: {CurrentLives} | Adding: {lives}");

			CurrentLives += lives;
			if (CurrentLives > MaximumLives)
			{
				CurrentLives = MaximumLives;
			}

			Debug.Log($"GainLives - New Life Count: {CurrentLives}");

			//*****************************************************************************************
			// Update UI text
			/*if (LifeCountText != null)
			{
				LifeCountText.text = $"Lives: {CurrentLives}";
			}
			else
			{
				Debug.LogWarning("LifeCountText is not assigned in the GameManager.");
			}*/
			//*****************************************************************************************

			// Fire the event so LivesCounter updates
			if (OnLivesChanged != null)
			{
				Debug.Log($"OnLivesChanged Subscribers: {OnLivesChanged.GetInvocationList().Length}");
				OnLivesChanged.Invoke();
				Debug.Log("OnLivesChanged event triggered successfully.");
			}
			else
			{
				Debug.LogWarning("OnLivesChanged event is NULL! UI will not update.");
			}
		}











		/// <summary>
		/// Use this method to increase the max amount of lives, and optionnally the current amount as well
		/// </summary>
		/// <param name="lives">Lives.</param>
		/// <param name="increaseCurrent">If set to <c>true</c> increase current.</param>
		public virtual void AddLives(int lives, bool increaseCurrent)
		{
			MaximumLives += lives;
			if (increaseCurrent) 
			{
				CurrentLives += lives;
			}
		}

		/// <summary>
		/// Resets the number of lives to their initial values.
		/// </summary>
		public virtual void ResetLives()
		{
			CurrentLives = _initialCurrentLives ;
			MaximumLives = _initialMaximumLives ;
		}
			
		/// <summary>
		/// Adds the points in parameters to the current game points.
		/// </summary>
		/// <param name="pointsToAdd">Points to add.</param>
		public virtual void AddPoints(int pointsToAdd)
		{
			Points += pointsToAdd;
			GUIManager.Instance.RefreshPoints ();
		}
		
		/// <summary>
		/// use this to set the current points to the one you pass as a parameter
		/// </summary>
		/// <param name="points">Points.</param>
		public virtual void SetPoints(int points)
		{
			Points = points;
			GUIManager.Instance.RefreshPoints ();
		}

		protected virtual void SetActiveInventoryInputManager(bool status)
		{
			_inventoryInputManager = GameObject.FindObjectOfType<InventoryInputManager> ();
			if (_inventoryInputManager != null)
			{
				_inventoryInputManager.enabled = status;
			}
		}
		
		/// <summary>
		/// Pauses the game or unpauses it depending on the current state
		/// </summary>
		public virtual void Pause(PauseMethods pauseMethod = PauseMethods.PauseMenu)
		{	
			if ((pauseMethod == PauseMethods.PauseMenu) && _inventoryOpen)
			{
				return;
			}

			// if time is not already stopped		
			if (Time.timeScale>0.0f)
			{
				MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 0f, 0f, false, 0f, true);
				Instance.Paused=true;
				if ((GUIManager.HasInstance) && (pauseMethod == PauseMethods.PauseMenu))
				{
					GUIManager.Instance.SetPause(true);	
					_pauseMenuOpen = true;
					SetActiveInventoryInputManager (false);
				}
				if (pauseMethod == PauseMethods.NoPauseMenu)
				{
					_inventoryOpen = true;
				}
			}
			else
			{
				UnPause(pauseMethod);
				CorgiEngineEvent.Trigger(CorgiEngineEventTypes.UnPause);
			}		
			LevelManager.Instance.ToggleCharacterPause();
		}

		/// <summary>
		/// Unpauses the game
		/// </summary>
		public virtual void UnPause(PauseMethods pauseMethod = PauseMethods.PauseMenu)
		{
			MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Unfreeze, 1f, 0f, false, 0f, false);
			Instance.Paused = false;
			if ((GUIManager.HasInstance) && (pauseMethod == PauseMethods.PauseMenu))
			{ 
				GUIManager.Instance.SetPause(false);
				_pauseMenuOpen = false;
				SetActiveInventoryInputManager (true);
			}
			if (_inventoryOpen)
			{
				_inventoryOpen = false;
			}
			LevelManager.Instance.ToggleCharacterPause();
		}

		/// <summary>
		/// Deletes all save files
		/// </summary>
		public virtual void ResetAllSaves()
		{
			MMSaveLoadManager.DeleteSaveFolder ("InventoryEngine");
			MMSaveLoadManager.DeleteSaveFolder ("CorgiEngine");
			MMSaveLoadManager.DeleteSaveFolder ("MMAchievements");
			MMSaveLoadManager.DeleteSaveFolder ("MMRetroAdventureProgress");
		}

		/// <summary>
		/// Stores the points of entry for the level whose name you pass as a parameter.
		/// </summary>
		/// <param name="levelName">Level name.</param>
		/// <param name="entryIndex">Entry index.</param>
		/// <param name="exitIndex">Exit index.</param>
		public virtual void StorePointsOfEntry(string levelName, int entryIndex, Character.FacingDirections facingDirection)
		{
			if (PointsOfEntry.Count > 0)
			{
				foreach (PointsOfEntryStorage point in PointsOfEntry)
				{
					if (point.LevelName == levelName)
					{
						point.FacingDirection = facingDirection;
						point.PointOfEntryIndex = entryIndex;
						return;
					}
				}	
			}

			PointsOfEntry.Add (new PointsOfEntryStorage (levelName, entryIndex, facingDirection));
		}

		/// <summary>
		/// Gets point of entry info for the level whose scene name you pass as a parameter
		/// </summary>
		/// <returns>The points of entry.</returns>
		/// <param name="levelName">Level name.</param>
		public virtual PointsOfEntryStorage GetPointsOfEntry(string levelName)
		{
			if (PointsOfEntry.Count > 0)
			{
				foreach (PointsOfEntryStorage point in PointsOfEntry)
				{
					if (point.LevelName == levelName)
					{
						return point;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Clears the stored point of entry infos for the level whose name you pass as a parameter
		/// </summary>
		/// <param name="levelName">Level name.</param>
		public virtual void ClearPointOfEntry(string levelName)
		{
			if (PointsOfEntry.Count > 0)
			{
				foreach (PointsOfEntryStorage point in PointsOfEntry)
				{
					if (point.LevelName == levelName)
					{
						PointsOfEntry.Remove (point);
					}
				}
			}
		}

		/// <summary>
		/// Clears all points of entry.
		/// </summary>
		public virtual void ClearAllPointsOfEntry()
		{
			PointsOfEntry.Clear ();
		}

		/// <summary>
		/// Sets a new persistent character
		/// </summary>
		/// <param name="newCharacter"></param>
		public virtual void SetPersistentCharacter(Character newCharacter)
		{
			PersistentCharacter = newCharacter;
		}

		/// <summary>
		/// Destroys a persistent character if there's one
		/// </summary>
		public virtual void DestroyPersistentCharacter()
		{
			if (PersistentCharacter != null)
			{
				Destroy(PersistentCharacter.gameObject);
				SetPersistentCharacter(null);
			}
			

			if (LevelManager.Instance.Players[0] != null)
			{
				if (LevelManager.Instance.Players[0].gameObject.MMGetComponentNoAlloc<CharacterPersistence>() != null)
				{
					Destroy(LevelManager.Instance.Players[0].gameObject);	
				}
			}
		}

		/// <summary>
		/// Stores the selected character for use in upcoming levels
		/// </summary>
		/// <param name="selectedCharacter">Selected character.</param>
		public virtual void StoreSelectedCharacter(Character selectedCharacter)
		{
			StoredCharacter = selectedCharacter;
		}

		/// <summary>
		/// Clears the selected character.
		/// </summary>
		public virtual void ClearStoredCharacter()
		{
			StoredCharacter = null;
		}

		/// <summary>
		/// Catches MMGameEvents and acts on them, playing the corresponding sounds
		/// </summary>
		/// <param name="gameEvent">MMGameEvent event.</param>
		public virtual void OnMMEvent(MMGameEvent gameEvent)
		{
			switch (gameEvent.EventName)
			{
				case "inventoryOpens":
					Pause (PauseMethods.NoPauseMenu);
					break;

				case "inventoryCloses":
					Pause (PauseMethods.NoPauseMenu);
					break;
			}
		}

		/// <summary>
		/// Catches CorgiEngineEvents and acts on them, playing the corresponding sounds
		/// </summary>
		/// <param name="engineEvent">CorgiEngineEvent event.</param>
		public virtual void OnMMEvent(CorgiEngineEvent engineEvent)
		{
			switch (engineEvent.EventType)
			{
				case CorgiEngineEventTypes.TogglePause:
					if (Paused)
					{
						CorgiEngineEvent.Trigger(CorgiEngineEventTypes.UnPause);
					}
					else
					{
						CorgiEngineEvent.Trigger(CorgiEngineEventTypes.Pause);
					}
					break;

				case CorgiEngineEventTypes.Pause:
					Pause ();
					break;
				
				case CorgiEngineEventTypes.UnPause:
					UnPause ();
					break;
				
				case CorgiEngineEventTypes.PauseNoMenu:
					Pause(PauseMethods.NoPauseMenu);
					break;
			}
		}

		/// <summary>
		/// Catches CorgiEnginePointsEvents and acts on them, playing the corresponding sounds
		/// </summary>
		/// <param name="pointEvent">CorgiEnginePointsEvent event.</param>
		public virtual void OnMMEvent(CorgiEnginePointsEvent pointEvent)
		{
			switch (pointEvent.PointsMethod)
			{
				case PointsMethods.Set:
					SetPoints (pointEvent.Points);
					break;

				case PointsMethods.Add:
					AddPoints (pointEvent.Points);
					break;
			}
		}

		/// <summary>
		/// OnDisable, we start listening to events.
		/// </summary>
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMGameEvent> ();
			this.MMEventStartListening<CorgiEngineEvent> ();
			this.MMEventStartListening<CorgiEnginePointsEvent> ();
			Cursor.visible = true;
		}

		/// <summary>
		/// OnDisable, we stop listening to events.
		/// </summary>
		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMGameEvent> ();
			this.MMEventStopListening<CorgiEngineEvent> ();
			this.MMEventStopListening<CorgiEnginePointsEvent> ();
		}
	}
}