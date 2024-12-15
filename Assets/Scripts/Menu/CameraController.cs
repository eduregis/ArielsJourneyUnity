using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public class Anchor
    {
        public string name; // Nome da âncora
        public GameObject gameObject; // GameObject associado à âncora
        public string up;   // Nome da âncora "para cima"
        public string down; // Nome da âncora "para baixo"
        public string left; // Nome da âncora "para a esquerda"
        public string right; // Nome da âncora "para a direita"
        
        // A posição será baseada no transform do GameObject
        public Vector3 position {
            get { return gameObject != null ? gameObject.transform.position : Vector3.zero; }
        }
    }

    public List<Anchor> anchors = new List<Anchor>(); // Lista de âncoras configuradas no Inspector
    public float moveSpeed = 5f; // Velocidade de movimentação da câmera
    private Anchor currentAnchor; // Âncora onde a câmera está atualmente
    private Vector2 startTouchPosition, endTouchPosition;
    private float swipeThreshold = 100f; // Distância mínima para um swipe ser reconhecido

    private void Start()
    {
        if (anchors.Count > 0)
        {
            currentAnchor = anchors[0]; // Inicializa com a primeira âncora da lista
            transform.position = currentAnchor.position; // Define a posição da câmera
        }
    }

    private void Update()
    {
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                Vector2 swipe = endTouchPosition - startTouchPosition;
                if (swipe.magnitude > swipeThreshold)
                {
                    if (Mathf.Abs(swipe.x) < Mathf.Abs(swipe.y))
                    {
                        if (swipe.y > 0)
                            MoveToAnchorByName(currentAnchor.up); // Swipe para cima
                        else
                            MoveToAnchorByName(currentAnchor.down); // Swipe para baixo
                    }
                    else
                    {
                        if (swipe.x > 0)
                            MoveToAnchorByName(currentAnchor.right); // Swipe para direita
                        else
                            MoveToAnchorByName(currentAnchor.left); // Swipe para esquerda
                    }
                }
            }
        }
    }

    private void MoveToAnchorByName(string anchorName)
    {
        // Se não houver nome de âncora para mover, não faz nada
        if (string.IsNullOrEmpty(anchorName)) return;

        // Procura a âncora pelo nome e move a câmera para a posição dela
        Anchor targetAnchor = anchors.Find(anchor => anchor.name == anchorName);
        
        if (targetAnchor != null)
        {
            currentAnchor = targetAnchor; // Atualiza a âncora atual
            StopAllCoroutines();
            StartCoroutine(MoveCamera(targetAnchor.position));
        }
    }

    private System.Collections.IEnumerator MoveCamera(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition; // Garante o alinhamento perfeito
    }
}
