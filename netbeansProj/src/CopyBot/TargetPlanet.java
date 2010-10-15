/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package CopyBot;

/**
 *
 * @author gwalker
 */
public class TargetPlanet {

    public TargetPlanet(PlanetWars pw, Planet p){
        if (p.Owner() == 0) NeutralForces = p.NumShips();
        else if (p.Owner() == 1) MyForces = p.NumShips();
        else EnemyForces = p.NumShips();
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
    public Owner owner = Owner.Neutral;

//    public void TurnEnded() {
//        EnemyEnrouteTotalDistance -= EnemyEnrouteFleets;
//        ShipsEnrouteTotalDistance -= ShipsEnrouteFleets;
//    }

    //If less than zero, it means I control it
    public int LongTermForces() {
        return (NeutralForces + EnemyForces + EnemyEnroute)
                - (MyForces + ShipsEnroute);
    }

    public enum AttackType {

        None,
        Defend,
        Attack,
        Copying
    }

    public enum Owner {

        Neutral,
        Me,
        Enemy
    }
}
