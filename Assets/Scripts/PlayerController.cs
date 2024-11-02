using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Mode pergerakan (Character Controller atau Rigidbody)
    private enum Mode { WithCC, WithRB }
    [SerializeField] private Mode _mode;

    // Komponen movement
    private TPMovementCC _tpMovementCC;
    private TPMovementRB _tpMovementRB;

    // Input
    private Vector2 _inputAxis;
    private bool _idle;
    private bool _walk;
    private bool _run;
    private bool _jump;

    private void Start()
    {
        // Cek komponen movement
        if (GetComponent<TPMovementCC>())
            _tpMovementCC = GetComponent<TPMovementCC>();
        else if (GetComponent<TPMovementRB>())
            _tpMovementRB = GetComponent<TPMovementRB>();
    }

    private void Update()
    {
        // Membaca input dari player
        ReadInput();

        // Memanggil metode update dari masing-masing mode
        if (_mode == Mode.WithCC)
        {
            _tpMovementCC.OnUpdate(_inputAxis);
            InputActionCC();
        }
        else
        {
            _tpMovementRB.OnUpdate(_inputAxis);
            InputActionRB();
        }
    }

    private void ReadInput()
    {
        _inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        _idle = _inputAxis.magnitude == 0;
        _walk = _inputAxis.magnitude < 0.5f && _inputAxis.magnitude > 0;
        _run = _inputAxis.magnitude > 0.5f;
        _jump = Input.GetButtonDown("Jump");
    }

    private void InputActionCC()
    {
        if (_idle)
            _tpMovementCC.Idle();
        else if (_walk)
            _tpMovementCC.Walk();
        else if (_run)
            _tpMovementCC.Run();

        if (_jump)
            _tpMovementCC.Jump();
    }

    private void InputActionRB()
    {
        if (_idle)
            _tpMovementRB.Idle();
        else if (_walk)
            _tpMovementRB.Walk();
        else if (_run)
            _tpMovementRB.Run();

        if (_jump)
            _tpMovementRB.Jump();
    }
}
