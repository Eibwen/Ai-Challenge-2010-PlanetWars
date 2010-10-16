/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package SpreadBot;

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

    public static Planet NearestPlanetNotOwnedCanTake(PlanetWars pw, PlanetDistances distances, int planetId, int avaliableForces) {
        Planet closest = null;
        int closeDistance = 1000;
        for (Planet p : pw.Planets()) {
            if (p.Owner() != 1) {
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
