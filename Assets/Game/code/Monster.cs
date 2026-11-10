using System.Linq;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject _model;
    [SerializeField] private ParticleSystem _pickupEffect;
    [SerializeField] private float _rotationSpeed = 50f;
    private bool _isUsed = false;

    private MonsterController _controller;

        public void OnTriggerEnter(Collider other)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();

            }
            _model.SetActive(false);
            _pickupEffect.Play();
            if(_isUsed)
            {
                return;
            }
            _isUsed = true;
            _controller.MonsterPickedCommand();
        }

        public void SetupMonster(MonsterController controller)
    {
        _controller = controller;
    }
    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }


        

    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
