using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Dictionary<String, bool> npcList = new Dictionary<String, bool>();

    public float movementSpeed;
    public float gravity;
    public float movementDrag;
    public float maxJumpHeight;

    public Text interactLemonStandText;
    public Text lemonadeCountText;
    public Text remainingLifeText;

    private int _numLemonade;
    private bool _nearStand;
    private bool _nearNpc;

    private GameObject _currentNpc;

    private float _horizontalInput;
    private float _rotationInput;

    public int remainingLife;
    public bool gameOver; // false when game still going, true when game over

    public GameObject lightSource;

    private int counter = 0;

    // Jumping
    private bool _jumpInput;
    private bool _clickButton;
    private bool _hasPressedJump;
    private bool _releasedJump;

    // The player metadata
    private Rigidbody _rigidbody;
    private Transform _transform;
    

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _transform = gameObject.GetComponent<Transform>();

        _hasPressedJump = false;
        _releasedJump = false;

        _numLemonade = 0;
        remainingLife = 100;
        gameOver = false;


        npcList.Add("Npc1", true);
        npcList.Add("Npc2", true);
        npcList.Add("Npc3", true);
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Vertical");
        _rotationInput = Input.GetAxis("Horizontal");
        _jumpInput = Input.GetButton("Jump");
        _clickButton = Input.GetButtonDown("Fire1");

        

        if (_nearStand && _clickButton)
        {
            _numLemonade++;
            lemonadeCountText.text = "Lemonade Count: " + _numLemonade;
        }

        if (remainingLife <= 0)
        {
            remainingLifeText.text = "You are dehydrated!";
            gameOver = true;
        }
        else
        {
            remainingLifeText.text = "Remaining Life : " + remainingLife;
        }

        if (!npcList["Npc1"] && !npcList["Npc2"] && !npcList["Npc3"])
        {
            interactLemonStandText.text = "You Win!";
        }
    }

    private void FixedUpdate()
    {
        // if in the shade, recharge life; 
        bool isUnderSun = Physics.Raycast(transform.position, lightSource.transform.position);
        if (!isUnderSun)
        {
            counter += 1;
            if (counter >= 20)
            { 
                remainingLife -= 2;
                counter = 0;
            }
        }
        else
        {
            counter -= 2;
            if (counter <= 0)
            {
                remainingLife += 5;
                counter = 0;
            }
        }
        // if in the sun, deplete life;
        ChangePlayerDirection();
        if (_jumpInput)
        {
            _hasPressedJump = true;
        }

        if ((_hasPressedJump && !_jumpInput) || transform.position.y > maxJumpHeight)
        {
            _releasedJump = true;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _releasedJump = false;
            _hasPressedJump = false;
        }

        else if (other.gameObject.CompareTag("Car"))
        {
            remainingLife = 1;
        }
    }

    void ChangePlayerDirection()
    {
        Vector3 newVelocity = _rigidbody.velocity;
        newVelocity.y -= gravity * Time.deltaTime;

        if (_horizontalInput != 0)
        {
            newVelocity += transform.forward * _horizontalInput * movementSpeed;
        }
        else
        {
            newVelocity.x = 0;
            newVelocity.z = 0;

            // newVelocity.x = reduceVelocity(newVelocity.x);
            // newVelocity.z = reduceVelocity(newVelocity.z);
        }

        _rigidbody.velocity = newVelocity;

        Vector3 userRotation = transform.rotation.eulerAngles;
        userRotation += new Vector3(0, _rotationInput, 0);
        _transform.rotation = Quaternion.Euler(userRotation);

        if (_jumpInput && !_releasedJump)
        {
            _rigidbody.AddForce(Vector3.up, ForceMode.VelocityChange);
        }
    }

    float reduceVelocity(float originalVelocity)
    {
        Debug.Log(Math.Abs(originalVelocity - (movementSpeed * Time.deltaTime)));
        if (Math.Abs(originalVelocity - (movementSpeed * Time.deltaTime)) < 1)
        {
            return 0f;
        }

        if (originalVelocity > 0)
        {
            return originalVelocity - movementDrag * Time.deltaTime;
        }

        if (originalVelocity < 0)
        {
            return originalVelocity + movementDrag * Time.deltaTime;
        }

        return originalVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LemonStand"))
        {
            interactLemonStandText.text = "Press E to get Lemonade";
            _nearStand = true;
        }
        else if (other.gameObject.CompareTag("Npc1") && _numLemonade > 0)
        {
            Debug.Log("Npc1");
            if (npcList["Npc1"])
            {
                _numLemonade--;
                lemonadeCountText.text = "Lemonade Count: " + _numLemonade;

            }

            npcList["Npc1"] = false;
        }
        else if (other.gameObject.CompareTag("Npc2") && _numLemonade > 0)
        {
            if (npcList["Npc2"])
            {
                _numLemonade--;
                lemonadeCountText.text = "Lemonade Count: " + _numLemonade;

            }

            npcList["Npc2"] = false;
        }
        else if (other.gameObject.CompareTag("Npc3") && _numLemonade > 0)
        {
            if (npcList["Npc3"])
            {
                _numLemonade--;
                lemonadeCountText.text = "Lemonade Count: " + _numLemonade;

            }

            npcList["Npc3"] = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LemonStand"))
        {
            interactLemonStandText.text = "";
            _nearStand = true;
        }
    }
}