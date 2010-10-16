/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Common;

import Common.*;
import java.util.HashMap;

/**
 *
 * @author gwalker
 */
public class PlanetDistances {

    public PlanetDistances(PlanetWars pw) {
        for (Planet p : pw.Planets()) {
            for (Planet q : pw.Planets()) {

                distances.put(p.PlanetID() * 100 + q.PlanetID(), pw.Distance(p.PlanetID(), q.PlanetID()));
            }
        }
    }

    public int Distance(int p1, int p2) {
        return distances.get(p1 * 100 + p2);
    }
    int Distance(Planet p1, Planet p2) {
        return distances.get(p1.PlanetID() * 100 + p2.PlanetID());
    }

    HashMap<Integer, Integer> distances = new HashMap<Integer, Integer>();
}
