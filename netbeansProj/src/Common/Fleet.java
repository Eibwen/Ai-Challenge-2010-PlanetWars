package Common;

import java.util.Comparator;

public class Fleet implements Comparable, Cloneable {
    // Initializes a fleet.

    public Fleet(int owner,
            int numShips,
            int sourcePlanet,
            int destinationPlanet,
            int totalTripLength,
            int turnsRemaining,
            int turnNum) {
        this.owner = owner;
        this.numShips = numShips;
        this.sourcePlanet = sourcePlanet;
        this.destinationPlanet = destinationPlanet;
        this.totalTripLength = totalTripLength;
        this.turnsRemaining = turnsRemaining;
        this.turnNumber = turnNum;
    }

    // Initializes a fleet.
    public Fleet(int owner,
            int numShips) {
        this.owner = owner;
        this.numShips = numShips;
        this.sourcePlanet = -1;
        this.destinationPlanet = -1;
        this.totalTripLength = -1;
        this.turnsRemaining = -1;
        this.turnNumber = -1;
    }

    // Accessors and simple modification functions. These should be mostly
    // self-explanatory.
    public int Owner() {
        return owner;
    }

    public int NumShips() {
        return numShips;
    }

    public int SourcePlanet() {
        return sourcePlanet;
    }

    public int DestinationPlanet() {
        return destinationPlanet;
    }

    public int TotalTripLength() {
        return totalTripLength;
    }

    public int TurnsRemaining() {
        return turnsRemaining;
    }

    public int ArivialTurn() {
        return turnsRemaining + turnNumber;
    }

    public int TurnsRemainingFuture(int turnNum) {
        return turnsRemaining - (turnNum - turnNumber);
    }

    public void RemoveShips(int amount) {
        numShips -= amount;
    }

    // Subtracts one turn remaining. Call this function to make the fleet get
    // one turn closer to its destination.
    public void TimeStep() {
        if (turnsRemaining > 0) {
            --turnsRemaining;
        } else {
            turnsRemaining = 0;
        }
    }

    @Override
    public int compareTo(Object o) {
        Fleet f = (Fleet) o;
        return this.numShips - f.numShips;
    }
    
    public static ComparatorShipCount comparatorShipCount() {
        return new ComparatorShipCount();
    }
    public static class ComparatorShipCount implements Comparator<Fleet> {
        public int compare(Fleet t, Fleet t1) {
            return t.numShips - t1.numShips;
        }
    }
    public static ComparatorTurnsRemaining comparatorTurnsRemaining() {
        return new ComparatorTurnsRemaining();
    }
    public static class ComparatorTurnsRemaining implements Comparator<Fleet> {
        public int compare(Fleet t, Fleet t1) {
            return t.turnsRemaining - t1.turnsRemaining;
        }
    }
    public static ComparatorTurnsRemainingRealitive comparatorTurnsRemainingRealitive() {
        return new ComparatorTurnsRemainingRealitive();
    }
    public static class ComparatorTurnsRemainingRealitive implements Comparator<Fleet> {
        public int compare(Fleet t, Fleet t1) {
            return (t.turnsRemaining + t.turnNumber) - (t1.turnsRemaining + t1.turnNumber);
        }
    }

    private int owner;
    private int numShips;
    private int sourcePlanet;
    private int destinationPlanet;
    private int totalTripLength;
    private int turnsRemaining;
    private int turnNumber;

    private Fleet(Fleet _f) {
        owner = _f.owner;
        numShips = _f.numShips;
        sourcePlanet = _f.sourcePlanet;
        destinationPlanet = _f.destinationPlanet;
        totalTripLength = _f.totalTripLength;
        turnsRemaining = _f.turnsRemaining;
        turnNumber = _f.turnNumber;
    }

    @Override
    public Object clone() {
        return new Fleet(this);
    }

    @Override
    public boolean equals(Object other) {
        // Not strictly necessary, but often a good optimization
        if (this == other) {
            return true;
        }
        if (!(other instanceof Fleet)) {
            return false;
        }
        Fleet otherFleet = (Fleet) other;
        return (this.owner == otherFleet.owner
                && this.numShips == otherFleet.numShips
                && this.sourcePlanet == otherFleet.sourcePlanet
                && this.destinationPlanet == otherFleet.destinationPlanet
                && this.ArivialTurn() == otherFleet.ArivialTurn()
                && this.totalTripLength == otherFleet.totalTripLength);
    }

    @Override
    public int hashCode() {
        int hash = 17;
        // Suitable nullity checks etc, of course :)
        hash = hash * 23 + this.owner;
        hash = hash * 23 + this.numShips;
        hash = hash * 23 + this.sourcePlanet;
        hash = hash * 23 + this.destinationPlanet;
        hash = hash * 23 + this.totalTripLength;
        hash = hash * 23 + this.ArivialTurn();

//        hash = hash * 23 ^ this.owner;
//        hash = hash * 23 ^ this.numShips;
//        hash = hash * 23 ^ this.sourcePlanet;
//        hash = hash * 23 ^ this.destinationPlanet;
//        hash = hash * 23 ^ this.totalTripLength;
//        hash = hash * 23 ^ this.turnsRemaining;

        return hash;
    }
}
