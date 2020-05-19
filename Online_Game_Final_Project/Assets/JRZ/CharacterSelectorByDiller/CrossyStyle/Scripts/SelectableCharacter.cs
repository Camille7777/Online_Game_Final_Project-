using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Selectable character.
/// </summary>
public class SelectableCharacter : MonoBehaviour {

	//Name given, and what will be displayed
	public string m_CharacterName;

	//For the locked and unlocked coloring of the sprite
	private Color unlockedColor = Color.white;
	private Color lockedColor = Color.black;

	//Storing info needed for controlling the lock states
	private string unlockSaveState;
	private int isUnlocked = 0;

	//Image of the character
	private Image characterImage;

	private void Start() {
		//this.LoadLockState();
		//this.CheckLockedState();
	}

	/// <summary>
	/// Checks the state of the locked.
	/// </summary>
	private void CheckLockedState() {
		if(isUnlocked == 0) {
			characterImage.color = lockedColor;
		}
		else {
			characterImage.color = unlockedColor;
		}
	}

	/// <summary>
	/// Loads the state of the lock.
	/// </summary>
	private void LoadLockState() {
		if(PlayerPrefs.HasKey(unlockSaveState)) {
			isUnlocked = PlayerPrefs.GetInt(unlockSaveState);
		}
	}

	/// <summary>
	/// Saves the state of the lock.
	/// </summary>
	private void SaveLockState() {
		PlayerPrefs.SetInt(unlockSaveState, isUnlocked);
		PlayerPrefs.Save();
	}

	/// <summary>
	/// Buies the character.
	/// </summary>
	public void BuyCharacter() {
		isUnlocked = 1;
		this.SaveLockState();
		this.CheckLockedState();
	}

	/// <summary>
	/// Loads the character.
	/// </summary>
	public void LoadCharacter() {
		characterImage = this.GetComponent<Image>();
		unlockSaveState = m_CharacterName + "_unlock";

		this.LoadLockState();
		this.CheckLockedState();
	}
}
