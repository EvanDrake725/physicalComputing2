using Godot;
using System;
using System.IO.Ports;

public partial class Player : CharacterBody2D
{
    SerialPort serialPort;
    bool hasHeardFromArduino = false;
     public float RotationSpeed = 5.0f;

    private float _targetRotation = 0.0f;

    public override void _Ready()
    {
        serialPort = new SerialPort("COM6", 9600) 
        { 
            ReadTimeout = 10,
            DtrEnable = true, 
            RtsEnable = true 
        };
        
        serialPort.Open();
    }

    [Export]
    public int Speed { get; set; } = 300;
    public int Health=10;
    public int prevValue=0;


    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDirection = Vector2.Zero;
        if (serialPort == null || !serialPort.IsOpen)
        return;
        try
        {
            while (serialPort.BytesToRead > 0)
            {
				string serialMessage = serialPort.ReadLine().Trim();

				if (serialMessage == "UP")
				{
					inputDirection.Y -= 1;
				}
				else if (serialMessage == "DOWN")
				{
					inputDirection.Y += 1;
				}
				else if (serialMessage == "RIGHT")
				{
					inputDirection.X += 1;
				}
				else if (serialMessage == "LEFT")
				{
					inputDirection.X -= 1;
				}
				else
				{
					int value = int.Parse(serialMessage);
					if (value < prevValue)
                    {
                        RotationDegrees+=10;
                    }
                    else if (value > prevValue)
                    {
                        RotationDegrees-=10;
                    }
                    prevValue = value;
				}                
            }
        }
        catch (TimeoutException)
        {
        }
        Velocity = inputDirection * Speed;
        MoveAndSlide();
    }
}