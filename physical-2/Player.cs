using Godot;
using System;
using System.IO.Ports;

public partial class Player : CharacterBody2D
{
    SerialPort serialPort;
    bool hasHeardFromArduino = false;
    [Export]
    public PackedScene BulletScene { get; set; }

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
    public int Health=6;
    public int prevValue=0;
    public bool briefcase=false;


    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDirection = Vector2.Zero;
        if (Health<=0)
        {
            serialPort.Close();
            GetTree().ChangeSceneToFile("res://Title.tscn");
        }
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
                else if (serialMessage == "ACTIVE")
                {
                    Area2D bullet = BulletScene.Instantiate<Area2D>();
                    bullet.GlobalPosition = GlobalPosition;
                    bullet.GlobalRotation = GlobalRotation;
                    GetTree().CurrentScene.AddChild(bullet);
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