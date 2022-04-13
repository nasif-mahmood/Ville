using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.PyroParticles
{
    public class PlayerAttacks : MonoBehaviour
    {
        private Rigidbody character;
        // for dog animator
        private Animator d_anim;

        // Getting the fireball object publically
        public GameObject fireballPrefab;

        // getting the sword object publically
        public GameObject swordPrefab;

        // holding the fireball object separately so original prefab won't be destroyed
        private GameObject fireballPrefabObject;

        private FireBaseScript fireballPrefabScript;

        // variables such that the fireball can't be spawned again until 2 seconds have passed
        float fireballDelay = 1.0f;
        float buttonPressTime;

        // variables to determine when the character attack animations are playing and for how long
        // should it be playing until you can attack again
        bool currentlyAttacking = false;
        // when the animation is 90% done
        float animTime = 0.9f;

        // Start is called before the first frame update
        void Start()
        {
            character = GetComponent<Rigidbody>();
            d_anim = GetComponentInChildren<Animator>();
        }


        // Update is called once per frame
        void Update()
        {
            // F key. Can only spawn another fireball once the delay time has passed
            if (Input.GetButtonDown("Fireball") && Time.time - buttonPressTime >= fireballDelay)
            {
                buttonPressTime = Time.time;
                Debug.Log("F button pushed");

                // If you are currently not attacking, set the animation trigger, give it time to
                // set, then start the fireball
                if(!currentlyAttacking)
                {
                    d_anim.SetTrigger("useFireball");
                    StartCoroutine(initializeAttack());
                    startFireball();
                }
            }

            // Left mouse click. Sword attack
            if (Input.GetButtonDown("Sword"))
            {
                Debug.Log("Left mouse pushed");

                // If you are currently not attacking, set the animation trigger, give it time to
                // set, then start the sword animation
                if(!currentlyAttacking)
                {
                    d_anim.SetTrigger("useSword");
                    StartCoroutine(initializeAttack());

                    // create a boxcollider that will exist around the sword only during attack
                    BoxCollider swordCollider = swordPrefab.AddComponent<BoxCollider>();
                    swordCollider.center = new Vector3(-0.01f, 0.7f, 0.01f);
                    swordCollider.size = new Vector3(0.2f, 1.3f, 0.07f);
                    Destroy(swordCollider, 0.4f);
                    
                }
            }

            // if you are attacking and the animation is about done (reached animTime) let the player attack again
            if (currentlyAttacking && d_anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= animTime)
            {
                currentlyAttacking = false;
            }
        }

        IEnumerator initializeAttack()
        {
            // give the trigger a moment to set
            yield return new WaitForSeconds(0.1f);
            currentlyAttacking = true;
        }

        void startFireball()
        {
            Vector3 projectilePos;

            // instantiate the rotation of the player
            Quaternion rotation = Quaternion.identity;

            // instantiate the proper object and script
            fireballPrefabObject = GameObject.Instantiate(fireballPrefab);

            fireballPrefabScript = fireballPrefabObject.GetComponent<FireBaseScript>();


            // set the start point near the player
            rotation = transform.rotation;
            projectilePos = transform.position + transform.forward + transform.right + transform.up;

            FireProjectileScript projectileScript = fireballPrefabObject.GetComponentInChildren<FireProjectileScript>();
            if (projectileScript != null)
            {
                // make sure we don't collide with other fire layers
                projectileScript.ProjectileCollisionLayers &= (~UnityEngine.LayerMask.NameToLayer("FireLayer"));
            }

            fireballPrefabObject.transform.position = projectilePos;
            fireballPrefabObject.transform.rotation = rotation;
        }
    }

}
