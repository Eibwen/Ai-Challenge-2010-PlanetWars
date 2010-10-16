/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package SimpleClosest;

import Common.*;
import java.util.Collection;

/**
 *
 * @author gwalker
 */
public class Find {

    public static Planet NearestPlanetNotOwned(PlanetWars pw, PlanetDistances distances, int planetId) {
        Planet closest = null;
        int closeDistance = 1000;
        for (Planet p : pw.Planets()) {
            if (p.Owner() != 1) {
                int distance = distances.Distance(p.PlanetID(), planetId);
                if (distance < closeDistance) {
                    closest = p;
                    closeDistance = distance;
                }
            }
        }
        return closest;
    }
    
     public static Planet NearestPlanetNotOwnedWeighted(PlanetWars pw, PlanetDistances distances, int planetId) {
        Planet closest = null;
        int closeDistance = 1000;
        for (Planet p : pw.Planets()) {
            if (p.Owner() != 1) {
                int distance = distances.Distance(p.PlanetID(), planetId);
                distance += p.NumShips() / p.GrowthRate();
                if (distance < closeDistance) {
                    closest = p;
                    closeDistance = distance;
                }
            }
        }
        return closest;
    }

    public static Planet NearestPlanetNotOwnedCanTake(Collection<Planet> planets, PlanetDistances distances, int planetId, int avaliableForces) {
        return NearestPlanetNotOwnedCanTake(planets, distances, planetId, avaliableForces, false);
    }

    public static Planet NearestPlanetNotOwnedCanTake(Collection<Planet> planets, PlanetDistances distances, int planetId, int avaliableForces, boolean OnlyEnemyControlled) {
        Planet closest = null;
        int closeDistance = 1000;
        for (Planet p : planets) {
            if (p.Owner() != 1
                && (!OnlyEnemyControlled || p.Owner() == 2)) {

                if (avaliableForces > p.NumShips()) {
                    int distance = distances.Distance(p.PlanetID(), planetId);
                    if (distance < closeDistance) {
                        closest = p;
                        closeDistance = distance;
                    }
                }
            }
        }
        return closest;
    }
}
