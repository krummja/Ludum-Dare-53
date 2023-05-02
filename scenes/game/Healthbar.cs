using Godot;


public partial class Healthbar : Control
{
    public static Color OK = new Color(1, 1, 1);
    public static Color HURT = new Color(0.5f, 0.5f, 0.5f);

    public Actor Player;
    public TextureRect H1;
    public TextureRect H2;
    public TextureRect H3;

    public TextureRect[] Hearts;

    public override void _Ready()
    {
        HUD HUD = GetParent<HUD>();
        Player = HUD.Player;

        H1 = GetNode<TextureRect>("H1");
        H2 = GetNode<TextureRect>("H2");
        H3 = GetNode<TextureRect>("H3");
        Hearts = new TextureRect[] { H1, H2, H3 };
    }

    public override void _Process(double delta)
    {
        for (int i = 0; i < 3; i++) {
            TextureRect heart = Hearts[i];
            heart.Modulate = i >= Player.Health ? HURT : OK;
        }
    }
}
