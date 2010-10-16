/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package SimpleClosest;

/**
 *
 * @author gwalker
 */
public class TargetPlanet implements Comparable {

    public TargetPlanet(PlanetWars pw, Planet p){
        PlanetId = p.PlanetID();
        if (p.Owner() == 0) NeutralForces = p.NumShips();
        else if (p.Owner() == 1) MyForces = p.NumShips();
        else EnemyForces = p.NumShips();
        GrowthRate = p.GrowthRate();
    }
    public void AddFleet(Fleet f){
        if (f.Owner()==1){
            //my fleet
            ShipsEnroute+=f.NumShips();
            ShipsEnrouteTotalDistance +=f.TurnsRemaining();
            ++ShipsEnrouteFleets;
        }
        else{
            EnemyEnroute+=f.NumShips();
            EnemyEnrouteTotalDistance +=f.TurnsRemaining();
            ++EnemyEnrouteFleets;
        }
    }

    public int PlanetId = -1;
    public int GrowthRate = 0;

    public int EnemyEnroute = 0;
    public int EnemyEnrouteTotalDistance = 0;
    public int EnemyEnrouteFleets = 0;

    public int ShipsEnroute = 0;
    public int ShipsEnrouteTotalDistance = 0;
    public int ShipsEnrouteFleets = 0;

    public int NeutralForces = 0;
    public int EnemyForces = 0;
    public int MyForces = 0;

    public AttackType attackType = AttackType.None;
    public int Owner = -1;

//    public void TurnEnded() {
//        EnemyEnrouteTotalDistance -= EnemyEnrouteFleets;
//        ShipsEnrouteTotalDistance -= ShipsEnrouteFleets;
//    }

    //If less than zero, it means I control it
    public int LongTermForces() {
        return (NeutralForces + EnemyForces + EnemyEnroute)
                - (MyForces + ShipsEnroute
                + EstimatedGrowth());
    }
    public int EnemyDistanceAverage(){
        if (EnemyEnrouteFleets == 0) return 0;
        return EnemyEnrouteTotalDistance / EnemyEnrouteFleets;
    }
    public int MyDistanceAverage(){
        if (ShipsEnrouteFleets == 0) return 0;
        return ShipsEnrouteTotalDistance / ShipsEnrouteFleets;
    }

    public int EstimatedGrowth() {
        if (Owner == 1) return -(MyDistanceAverage() * GrowthRate);
        if (Owner == -1) return 0;
        return EnemyDistanceAverage() * GrowthRate;
    }

    public int compareTo(Object o) {
        TargetPlanet t = (TargetPlanet) o;
        //if (this.MyForces > t.MyForces) return -1;
        return this.LongTermForces() - t.LongTermForces();
    }

    public enum AttackType {

        None,
        Defend,
        Attack,
        Copying
    }
}
