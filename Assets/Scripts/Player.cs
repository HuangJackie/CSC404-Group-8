using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float movementSpeed;
    public float gravity;
    public float movementDrag;
    public float maxJumpHeight;

    public Text interactLemonStandText;
    public Text lemonadeCountText;

    private int _numLemonade;
    private bool _nearStand;

    private float _horizontalInput;
    private float _rotationInput;

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
    }

    private void FixedUpdate()
    {
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