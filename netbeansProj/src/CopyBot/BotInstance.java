/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package CopyBot;

import Common.*;
import java.io.FileWriter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;
import java.util.TreeSet;

/**
 *
 * @author gwalker
 */
public class BotInstance {
    // The DoTurn function is where your code goes. The PlanetWars object
    // contains the state of the game, including information about all planets
    // and fleets that currently exist. Inside this function, you issue orders
    // using the pw.IssueOrder() function. For example, to send 10 ships from
    // planet 3 to planet 8, you would say pw.IssueOrder(3, 8, 10).
    //
    // There is already a basic strategy in place here. You can use it as a
    // starting point, or you can throw it out entirely and replace it with
    // your own. Check out the tutorials and articles on the contest website at
    // http://www.ai-contest.com/resources.

    PlanetDistances distances = null;
//    List<Fleet> counteredFleets = new ArrayList<Fleet>();
    int turnNum = 0;

    public void DoTurn(PlanetWars pw) {

        if (distances == null) {
            distances = new PlanetDistances(pw);
        }

        write("==TURN " + ++turnNum);

        Boolean OnlyEnemyControlled = false;
        if (pw.Production(1) > (pw.Production(2) * 1.75)){
            //go in for the kill
            OnlyEnemyControlled = true;
        }

        LoadTargetPlanets(pw, OnlyEnemyControlled);

        if (pw.EnemyFleets().size() > 0) {
            for (TargetPlanet tp : targets) {

                write("Planet: " + tp.PlanetId + " needs: " + tp.LongTermForces());

                //Want to go from small to large... skipping over negatives
                if (tp.LongTermForces() > 0) {
                    //if target is owned = UNDER ATTACK
                    int NeedShips = tp.LongTermForces();
                    TreeSet<Forces> forces = Forces.GetForces(pw, distances, tp.PlanetId, tp.EnemyDistanceAverage());

                    //TODO take into account ships enroute, and ships on target planet if i own it

                    int SentShips = 0;

                    for (Forces f : forces) {
                        if (NeedShips > 0) {

                            int takingShips = f.AvalibleLeaveDefendingForce();
                            if (takingShips > 0) {
                                ////write("Planet " + f.Planet + ": TakingShips: " + takingShips + " " + f.AvaliableForces + "  ");

                                //If this one finishes it off
                                if (NeedShips < takingShips) {
                                    //TODO NEED SHIP PADDING
                                    takingShips = NeedShips + 4;
                                }

                                pw.IssueOrder(f.Planet, tp.PlanetId, takingShips);
                                pw.GetPlanet(f.Planet).NumShips(f.AvaliableForces - takingShips);
                                NeedShips -= takingShips;
                                SentShips += takingShips;
                            }
                        }
                    }

                    write("Sent: " + SentShips + " to " + tp.PlanetId + ", wanted: " + NeedShips);
                }
            }
        } else {
            //Some sort of basic attack incase they are waiting for me to make the first move
            for (Planet p : pw.MyPlanets()) {
                Planet attack = Find.NearestPlanetNotOwned(pw.Planets(), distances, p.PlanetID());
                if (attack != null && p.NumShips() > 20) {
                    pw.IssueOrder(p.PlanetID(), attack.PlanetID(), p.NumShips() - 15);
                }
            }
        }




//        // (1) If we currently have a fleet in flight, just do nothing.
//        if (pw.MyFleets().size() >= 2) {
//            return;
//        }
//        // (2) Find my strongest planet.
//        Planet source = null;
//        double sourceScore = Double.MIN_VALUE;
//        for (Planet p : pw.MyPlanets()) {
//            double score = (double) p.NumShips();
//            if (score > sourceScore) {
//                sourceScore = score;
//                source = p;
//            }
//        }
//        // (3) Find the weakest enemy or neutral planet.
//        Planet dest = null;
//        double destScore = Double.MIN_VALUE;
//        for (Planet p : pw.NotMyPlanets()) {
//            double score = 1.0 / (1 + p.NumShips());
//            if (score > destScore) {
//                destScore = score;
//                dest = p;
//            }
//        }
//        // (4) Send half the ships from my strongest planet to the weakest
//        // planet that I do not own.
//        if (source != null && dest != null) {
//            int numShips = source.NumShips() / 2;
//            pw.IssueOrder(source, dest, numShips);
//        }
    }


    TreeSet<TargetPlanet> targets = null;
    public void LoadTargetPlanets(PlanetWars pw, Boolean OnlyEnemyControlled){
        TreeMap<Integer, TargetPlanet> planetTargets = new TreeMap<Integer, TargetPlanet>();
        //Add all planets first
        for (Planet p : pw.Planets()){
            if (!OnlyEnemyControlled || p.Owner() > 1) {
                planetTargets.put(p.PlanetID(), new TargetPlanet(p));
            }
        }
        //Build up the Fleets for each planet
        for (Fleet f : pw.Fleets()){
            if (planetTargets.containsKey(f.DestinationPlanet())) {
                planetTargets.get(f.DestinationPlanet()).AddFleet(f);
            }
        }

        targets = new TreeSet<TargetPlanet>();
        targets.addAll(planetTargets.values());
    }


//    //Attack Closest... no matter if i can beat it or not
//    //TODO make it attack the closest that
//    // problem with this is it just ends all the ships to the planet, then also sends while that fleet is traveling
//    public void DoTurn(PlanetWars pw) {
//        if (distances == null) {
//            distances = new PlanetDistances(pw);
//        }
//
//        for (Planet p : pw.MyPlanets()) {
//            //Planet attack = Find.NearestPlanetNotOwned(pw, distances, p.PlanetID());
//            //Planet attack = Find.NearestPlanetNotOwnedWeighted(pw, distances, p.PlanetID());
//            Planet attack = Find.NearestPlanetNotOwnedCanTake(pw, distances, p.PlanetID(), p.NumShips() - 10);
//            if (attack != null && p.NumShips() > 20) {
//                pw.IssueOrder(p.PlanetID(), attack.PlanetID(), p.NumShips() - 15);
//            }
//        }
//    }
    private static String logFile = "D:\\Projects\\CSharp\\!Personal\\GoogleAIChallenge2010\\Bots\\CopyBotLog.txt";

    public static void write(String msg) {
        write(logFile, msg);
    }

    public static void write(String file, String msg) {
        try {
            FileWriter aWriter = new FileWriter(file, true);
            aWriter.write(msg
                    + System.getProperty("line.separator"));
            aWriter.flush();
            aWriter.close();
        } catch (Exception e) {
        }
    }
}
