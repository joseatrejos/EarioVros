﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Platform2DUtils.GameplaySystem;
using UnityEngine.SceneManagement;

public class Player : Character2D
{   
    private GameManager gameManager;
    // Spawn point of the player
    private Vector2 spawnPoint;


    // Double Jump
    bool doubleJump = true;

        
    [SerializeField]
    public int lifes;

    // Sound Effects
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip gameOverSound;
    

    [SerializeField]
    int lives = 3;

    void Start()
    {
      /*  string path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path))
        {
            GameManager.instance.GameData = SaveSystem.LoadGameState();

            GameManager.instance.Player = gameObject; 

            GameManager.instance.PlayerPos = new Vector3(
                GameManager.instance.GameData.position[0],
                GameManager.instance.GameData.position[1]
            );

            GameManager.instance.Player.transform.position = GameManager.instance.PlayerPos;
        } else {
            Debug.Log("Save file not found in " + path);
        }*/
        
        gameManager = FindObjectOfType<GameManager>();
        gameManager.lastCheckPointPos = new Vector2(transform.position.x, transform.position.y);
    }
    
    void FixedUpdate()
    {
        if(GameplaySystem.JumpBtn)
        {
             if (Grounding) 
             {
                // anim.SetTrigger("jump");
                GameplaySystem.Jump(rb2D, jumpForce);
                doubleJump = true;
             }else {
                if (doubleJump) 
                {
                // anim.SetTrigger("jump2");
                GameplaySystem.Jump(rb2D, (jumpForce));
                doubleJump = false;
                }
            }
        //anim.SetBool("grounding", Grounding);
         }
    }

    void Update()
    {
        GameplaySystem.TMovementDelta(transform, moveSpeed);
    }

    void LateUpdate()
    {
        spr.flipX = FlipSprite;
        //anim.SetFloat("axisX", Mathf.Abs(GameplaySystem.Axis.x));
    }




    /// <summary>
    /// Fades the camera to the death screen and stops the music.
    /// </summary>
    void Death()
    {
        gameManager.lastCheckPointPos = gameManager.initialPosition;
        gameManager.dead = true;
        Destroy(this.gameObject);
        // I still need to make the camera fadeOut
         //SoundManager.instance.PlaySingle(gameOverSound);
        //SoundManager.instance.musicSource.Stop();
    }
   public void Hit()
    {
        lifes--;
        this.transform.position = new Vector2(gameManager.lastCheckPointPos.x, gameManager.lastCheckPointPos.y);
        Debug.Log(lifes);
        if(lifes<1)
        {
            Death();    
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Corazon"))
        {
            GameManager.instance.AddHeart();
            Destroy(other.gameObject);
        }
        if(other.CompareTag("Enemy"))
        {
            GameManager.instance.UpdateLives(-1);
        }
    }

    void Damage()
    {
        if(lives > 0)
        {
            GameManager.instance.UpdateLives(-1);
        } else
        {
            Death();
        }
    }
}
