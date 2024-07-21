using UnityEngine;

public class MoveParticlesController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveParticles;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    private ParticleSystem.MainModule _particleMainModule;
    private Color _currentFloorColor = Color.white;

    private void Start()
    {
        if (_playerController == null)
            _playerController = GetComponent<PlayerController>();

        if (_playerMovement == null)
            _playerMovement = GetComponent<PlayerMovement>();

        _particleMainModule = _moveParticles.main;
        _moveParticles.Play();
    }

    private void Update()
    {
        if (_playerController == null || _playerMovement == null) return;

        DetectGroundColor();
        HandleMoveParticles();
    }

    private void HandleMoveParticles()
    {
        var isMoving = _playerMovement.IsMoving();
        var inputStrength = isMoving ? 1f : 0f;

        _moveParticles.transform.localScale = Vector3.MoveTowards(_moveParticles.transform.localScale, Vector3.one * inputStrength, 2 * Time.deltaTime);

        if (isMoving && !_moveParticles.isPlaying)
            _moveParticles.Play();
        else if (!isMoving && _moveParticles.isPlaying)
            _moveParticles.Stop();
    }

    private void DetectGroundColor()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _groundLayer);

        if (hit.collider != null)
        {
            SpriteRenderer groundSprite = hit.collider.GetComponent<SpriteRenderer>();
            if (groundSprite != null)
            {
                _currentFloorColor = groundSprite.color;
                UpdateParticleColor();
            }
        }
    }

    private void UpdateParticleColor()
    {
        Color particleColor = new Color(
            _currentFloorColor.r * Random.Range(0.8f, 1f),
            _currentFloorColor.g * Random.Range(0.8f, 1f),
            _currentFloorColor.b * Random.Range(0.8f, 1f),
            1f
        );

        _particleMainModule.startColor = particleColor;
    }
}