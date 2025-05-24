

public enum EDimension
{
    BLUE,
    YELLOW,
    RED,
    GREEN

}

public static class Extensions
{
    public static EDimension getNext(this EDimension dim)
    {
        switch(dim){
            case EDimension.BLUE: return EDimension.YELLOW;
            case EDimension.YELLOW: return EDimension.RED;
            case EDimension.RED: return EDimension.GREEN;
            case EDimension.GREEN: return EDimension.BLUE;
        }
        return EDimension.GREEN;
    }
}
