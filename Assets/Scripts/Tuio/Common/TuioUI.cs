using UnityEngine;
using UnityEngine.UI;

namespace TuioSimulator.Tuio.Common
{
    [RequireComponent(typeof(Image))]
    public class TuioUI : MonoBehaviour
    {
        [SerializeField] private Shader _shader;

        private Image _image;
        private Material _material;

        private static readonly int Lifted = Shader.PropertyToID("_Lifted");
        
        private void Start()
        {
            _image = GetComponent<Image>();
            _material = new Material(_shader)
            {
                color = Color.blue
            };
            _image.material = _material;
        }

        public void SetGrounded(bool grounded)
        {
            var lifted = grounded ? 0 : 1;
            _material.SetInt(Lifted, lifted);
        }
    }
}