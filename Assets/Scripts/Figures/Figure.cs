using Utils;

public class Figure
{
    public FigureType Type;
    public readonly Team Team;
    public readonly string Sign;

    public Figure(FigureType type, Team team)
    {
        Type = type;
        Team = team;
    }

    public Figure(FigureType type, Team team, string s)
    {
        Type = type;
        Team = team;
        Sign = s;
    }
}