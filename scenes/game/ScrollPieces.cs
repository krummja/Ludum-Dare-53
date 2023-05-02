using Godot;


public partial class ScrollPieces : Control
{
    public static Color OBTAINED = new Color(1, 1, 1);
    public static Color UNOBTAINED = new Color(0.5f, 0.5f, 0.5f);

    public Actor Player;
    public TextureRect H1;
    public TextureRect H2;
    public TextureRect H3;

    public TextureRect[] Pieces;

    public override void _Ready()
    {
        HUD HUD = GetParent<HUD>();
        Player = HUD.Player;

        H1 = GetNode<TextureRect>("S1");
        H2 = GetNode<TextureRect>("S2");
        H3 = GetNode<TextureRect>("S3");
        Pieces = new TextureRect[] { H1, H2, H3 };
    }

    public override void _Process(double delta)
    {
        for (int i = 0; i < 3; i++) {
            TextureRect piece = Pieces[i];
            piece.Modulate = i < Player.ScrollPieces ? OBTAINED : UNOBTAINED;
        }
    }
}
