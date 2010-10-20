/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package MirrorBot;

import Common.*;
import java.util.*;

/**
 *
 * @author gwalker
 */
public class MirrorBot extends BotBase {

    Map<Integer, Planet> mirrorMap = null;

    Set<Fleet> mirroredFleets = new HashSet<Fleet>();
    PlanetDistances distances = null;

    int turnNum = 0;

    @Override
    public void DoTurn(PlanetWars pw) {

        write("==TURN " + ++turnNum);
        
        if (mirrorMap == null) {
            write("==Generating mirrorMap");
            mirrorMap = new HashMap<Integer, Planet>();

            double mapCenterX = 0;
            double mapCenterY = 0;

            for(Planet p : pw.Planets()){
                mapCenterX += p.X();
                mapCenterY += p.Y();
            }

            mapCenterX = mapCenterX / pw.Planets().size();
            mapCenterY = mapCenterY / pw.Planets().size();

            //write("Map Center: " + "(" + mapCenterX + ", " + mapCenterY + ")");

            HashMap<Point, Planet> mirroredLocation = new HashMap<Point, Planet>();

            for(Planet p : pw.Planets()){
                if (p.X() == mapCenterX && p.Y() == mapCenterY) {
                    write("-- Detected Center Planet --");
                    mirrorMap.put(p.PlanetID(), p);
                    continue;
                }

                Point mirroredPoint = new Point(p.X(),p.Y());

                //write("Looking for point of: " + mirroredPoint + " for " + p.PlanetID());
                if (mirroredLocation.containsKey(mirroredPoint)) {
                    Planet p1 = mirroredLocation.get(mirroredPoint);

                    write("-- Detected Mirror Planet: " + p.PlanetID() + " = " + p1.PlanetID() + " --");

                    mirrorMap.put(p1.PlanetID(), p);
                    mirrorMap.put(p.PlanetID(), p1);
                }
                else {
                    mirroredPoint.x = mapCenterX + mapCenterX - p.X();
                    mirroredPoint.y = mapCenterY + mapCenterY - p.Y();
                    
                    mirroredLocation.put(mirroredPoint, p);

                    //write("Set mirror point of: " + mirroredPoint + " for " + p.PlanetID());
                }
            }
            write("--Generated mirrorMap");
        }

        if (pw.EnemyFleets().size() > 0) {
            write("Checking Enemy Fleets");

            List<Fleet> fleets = pw.EnemyFleets();
            for (Fleet o : mirroredFleets) {
                fleets.remove(o);
            }

            write("Fleet count: " + fleets.size() + " -- mirrored: " + mirroredFleets.size());

            for (Fleet f : fleets) {
                    boolean handled = false;

                    Planet src = mirrorMap.get(f.SourcePlanet());
                    Planet dest = mirrorMap.get(f.DestinationPlanet());
                    //write("lamo: " + f.SourcePlanet() + " " + f.DestinationPlanet());
                    //write("lame: " + src.PlanetID() + " " + dest.PlanetID());
                    //write("owner: " + src.Owner() + " " + dest.Owner());
                    //Don't trade fleets between mirrored planets:
                    if (src.PlanetID() != f.DestinationPlanet()) {

                        write("Ships: " + src.PlanetID() + " -- " + src.NumShips() + " - " + f.NumShips());
                        if (src.NumShips() > f.NumShips() &&
                            src.Owner() == 1) {
                            pw.IssueOrder(src, dest, f.NumShips());
                            //src.NumShips(src.NumShips() - f.NumShips());
                            handled = true;
                        }

                    }
                    else {
                        handled = true;
                    }
                    if (handled) mirroredFleets.add(f);
//                }
//                else {
//                    write("Already handled fleet");
//                }
            }
        }
        else {
            write("Waiting");
//            write("Attacking Something");
//            if (distances == null) {
//                distances = new PlanetDistances(pw);
//            }
//
//            //Attack something
//            for (Planet p : pw.MyPlanets()) {
//                Planet attack = Find.NearestPlanetNotOwnedCanTake(pw.Planets(), distances, p.PlanetID(), p.NumShips() - 10);
//                if (attack != null && p.NumShips() > 20) {
//                    pw.IssueOrder(p.PlanetID(), attack.PlanetID(), p.NumShips() - 15);
//                }
//            }
        }
    }

}
