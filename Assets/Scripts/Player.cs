using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Public Variables
    [Header("Game Objects")]
    [SerializeField]private GameObject _circleToInstantiate;

    [Header("Physics Components")]
    [SerializeField] private float _jumpForce = 10f;

    [Header("Colors")]
    public Color _colorCyan;
    public Color _colorYellow;
    public Color _colorMagenta;
    public Color _colorPink;

    [Header("UI Components")]
    [SerializeField]private Text _scoreCounter;
    #endregion

    #region Private Variables
    private GameObject _currentCircle;
    private GameObject _oldCircle;
    private Rigidbody2D _rB;
    private SpriteRenderer _sR;
    private int _scoreInteger;
    private int _triggerCount;
    private string _currentColor;
    private bool _isGameStarted = false;
    #endregion

    #region Private Main Methods

    private void Awake()
    {
        _rB = GetComponent<Rigidbody2D>();
        _sR = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _scoreCounter.text = _scoreInteger.ToString();
        _rB.gravityScale = 0;
        SetColor();
        _currentCircle= Instantiate(_circleToInstantiate, new Vector2(_circleToInstantiate.transform.position.x, _circleToInstantiate.transform.position.y), Quaternion.identity);
        GameObject colorChanger = _currentCircle.transform.GetChild(0).gameObject;
        colorChanger.GetComponent<CircleRotater>()._rotateSpeed = 100;
        StartCoroutine(StartGame(1.6f));
    }


    private void Update()
    {
        PlayerMovemenet();
    }

    private IEnumerator StartGame(float duration)
    {
        yield return new WaitForSeconds(duration);
        _rB.gravityScale = 3;
        _isGameStarted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ColorChanger")
        {
            SetColor();
            SetScore();
            Destroy(collision.gameObject);
            return;
        }

        if (collision.tag != _currentColor)
            RestartScene();

        else if (collision.tag == _currentColor && _triggerCount != 1)
        {
            _triggerCount++;
            if (!collision.GetComponent<TriggerBool>()._isTriggerDetected)
            {
                collision.GetComponent<TriggerBool>()._isTriggerDetected = true;
            }
            else if (collision.GetComponent<TriggerBool>()._isTriggerDetected) return;
        }

        else if (collision.tag == _currentColor && _triggerCount == 1)
        {
            _oldCircle = _currentCircle;
            _currentCircle = Instantiate(_oldCircle, new Vector2(_oldCircle.transform.position.x, _oldCircle.transform.position.y + 8f), Quaternion.identity);
            _currentCircle.GetComponentInChildren<CircleRotater>()._rotateSpeed = 100;
            Destroy(_oldCircle, 5f);
            _triggerCount = 0;
        }
    }
    #endregion

    #region Private Helper Methods
    /// <summary>
    /// Control player movement with mouse click and tapping
    /// Called in update method.
    /// </summary>
    private void PlayerMovemenet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!_isGameStarted) return;
            else
                _rB.velocity = Vector2.up * _jumpForce;
        }
    }
    /// <summary>
    /// Set random color for player.
    /// called in Start method and after collision with ColorChanger.
    /// </summary>
    private void SetColor()
    {
        int index = Random.Range(0, 4);
        switch (index)
        {
            case 0:
                _currentColor = "Cyan";
                _sR.color = _colorCyan;
                break;
            case 1:
                _currentColor = "Yellow";
                _sR.color = _colorYellow;
                break;
            case 2:
                _currentColor = "Magenta";
                _sR.color = _colorMagenta;
                break;
            case 3:
                _currentColor = "Pink";
                _sR.color = _colorPink;
                break;
        }
    }
    /// <summary>
    /// Restart scene when collision with DeathPoint and wrong collision.
    /// </summary>
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// Set score after right collision.
    /// </summary>
    private void SetScore()
    {
        _scoreInteger = _scoreInteger + 1;
        _scoreCounter.text = _scoreInteger.ToString();
    }
    #endregion
}
