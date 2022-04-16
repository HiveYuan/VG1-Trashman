using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trashman {
    public class SoundManager : MonoBehaviour {

        public static SoundManager instance;

        //outlets
        AudioSource audioSource;
        public AudioClip winSound;
        public AudioClip buttonClickSound;
        public AudioClip loseSound;
        public AudioClip swordSound;
        public AudioClip coinSound;
        public AudioClip drinkSound;
        public AudioClip eat4Sound;
        public AudioClip eat1Sound;
        public AudioClip hit1Sound;
        public AudioClip hit3Sound;
        public AudioClip pickUpFoodSound;
        public AudioClip pickUpToolSound;
        public AudioClip explosionSound;

        void Awake() {
            instance = this;
        }

        // Start is called before the first frame update
        void Start() {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update() {

        }

        public void PlaySoundWin() {
            audioSource.PlayOneShot(winSound);
        }
        public void PlaySoundLose() {
            audioSource.PlayOneShot(loseSound);
        }
        public void PlaySoundButtonClick() {
            audioSource.PlayOneShot(buttonClickSound);
        }
        public void PlaySoundSword() {
            audioSource.PlayOneShot(swordSound);
        }
        public void PlaySoundHit1() {
            audioSource.PlayOneShot(hit1Sound);
        }
        public void PlaySoundHit3() {
            audioSource.PlayOneShot(hit3Sound);
        }
        public void PlaySoundCoin() {
            audioSource.PlayOneShot(coinSound);
        }
        public void PlaySoundDrink() {
            audioSource.PlayOneShot(drinkSound);
        }
        public void PlaySoundEat1() {
            audioSource.PlayOneShot(eat1Sound);
        }
        public void PlaySoundEat4() {
            audioSource.PlayOneShot(eat4Sound);
        }
        public void PlaySoundFoodPickup() {
            audioSource.PlayOneShot(pickUpFoodSound);
        }
        public void PlaySoundToolPickup() {
            audioSource.PlayOneShot(pickUpToolSound);
        }
        public void PlaySoundExplosion() {
            audioSource.PlayOneShot(explosionSound);
        }
    }
}