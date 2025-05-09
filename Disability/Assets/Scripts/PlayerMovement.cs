using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSmoothTime;
    public float GravityStrength;
    public float JumpStrength;
    public float WalkSpeed;
    public float RunSpeed;

    private CharacterController controller;
    private Vector3 currentMoveVelocity;
    private Vector3 moveDampVelocity;
    private Vector3 currentForceVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Récupération des inputs du joueur
        Vector3 playerInput = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical")
        };

        // Normalisation du mouvement pour éviter de se déplacer plus vite en diagonale
        if (playerInput.magnitude > 1f)
        {
            playerInput.Normalize();
        }

        // Calcul de la direction et de la vitesse
        Vector3 moveVector = transform.TransformDirection(playerInput);
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;

        // Lissage du mouvement
        currentMoveVelocity = Vector3.SmoothDamp(
            currentMoveVelocity,
            moveVector * currentSpeed,
            ref moveDampVelocity,
            MoveSmoothTime
        );

        // Applique le mouvement
        controller.Move(currentMoveVelocity * Time.deltaTime);

        // Vérification si le joueur est au sol
        Ray groundCheckRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundCheckRay, 1.1f))
        {
            // Réinitialise la vitesse verticale
            currentForceVelocity.y = -2f;

            // Saut
            if (Input.GetKey(KeyCode.Space))
            {
                currentForceVelocity.y = JumpStrength;
            }
        }
        else
        {
            // Applique la gravité
            currentForceVelocity.y -= GravityStrength * Time.deltaTime;
        }

        // Applique les forces verticales
        controller.Move(currentForceVelocity * Time.deltaTime);
    }
}

