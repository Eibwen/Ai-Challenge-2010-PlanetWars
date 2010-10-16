/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package SpreadBot;

import java.util.TreeSet;

/**
 *
 * @author gwalker
 */
public class Forces implements Comparable {

    public Forces(int planet, int planetGrowth, int avaliableForces) {
        Planet = planet;
        PlanetGrowth = planetGrowth;
        AvaliableForces = avaliableForces;
    }
    public int Planet = -1;
    public int PlanetGrowth = -1;
    public int AvaliableForces = -1;
    public int DistanceHandicap = 0;

    public int EffectiveForces() {
        return AvaliableForces - DistanceHandicap;
    }

    // TODO pulling a function out of my ass for this...
    public int AvalibleLeaveDefendingForce() {
        //this would say after 2 turns returns to 20
        return AvaliableForces - ((10 - PlanetGrowth) * 2);
    }

    // TODO Use a better fucntion for this?
    public int DistanceWeightedForces() {
        return AvaliableForces - (DistanceHandicap * DistanceHandicap);
    }

    @Override
    public int compareTo(Object o) {
        Forces f = (Forces) o;
        return this.DistanceWeightedForces() - f.DistanceWeightedForces();
    }

    //Look at my planets, check distance from it to target
    //find count of ships on that planet
    //(distance - ignoreGrowthForTurns) * growth = needed ships
    public static TreeSet<Forces> GetForces(PlanetWars pw, PlanetDistances distances, int target, /*int minimumCount, */ int ignoreGrowthForTurns) {

//TODO
        //TODO need to know how many ships are enroute already, and don't send an excessive amount
        TreeSet<Forces> ts = new TreeSet<Forces>();

        for (Planet p : pw.Planets()) {
            if (p.Owner() == 1) {
                //my planet

                Forces force = new Forces(p.PlanetID(), p.GrowthRate(), p.NumShips());

                int distance = distances.Distance(p.PlanetID(), target);
                if (distance > ignoreGrowthForTurns) {
                    //Needs handicap
                    force.DistanceHandicap = (distance - ignoreGrowthForTurns) * pw.GetPlanet(target).GrowthRate();
                }

                ts.add(force);
            }
        }

        return ts;
    }
}
