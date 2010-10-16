/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package SpreadBot;

import Common.*;
import java.io.FileWriter;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
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

    final static boolean Debugging = false;

    PlanetDistances distances = null;
    List<Fleet> counteredFleets = new ArrayList<Fleet>();
    int turnNum = 0;

    public void DoTurn(PlanetWars pw) {

        if (distances == null) {
            distances = new PlanetDistances(pw);
        }

        if (turnNum == 0 && pw.MyPlanets().size() > 0) {
            Planet home = null;
            Planet enemyHome = null;
            for (Planet findHome : pw.Planets()) {
                if (findHome.Owner() == 1) {
                    home = findHome;
                    if (enemyHome != null) break;
                }
                else if (findHome.Owner() == 2) {
                    enemyHome = findHome;
                    if (home != null) break;
                }
            }
            TreeMap<Integer, Planet> valueSorted = new TreeMap<Integer, Planet>(Collections.reverseOrder());

//            Planet distanceFrom = home;

//            int farthestPlanet = 0;
//            for (Planet p : pw.NeutralPlanets()) {
//                int dist = distances.Distance(distanceFrom, p);
//                if (dist > farthestPlanet) farthestPlanet = dist;
//            }
//            ++farthestPlanet;  //So the farthest one doesn't get multipled by 0

            for (Planet p : pw.NeutralPlanets()) {
//                int value = (farthestPlanet - distances.Distance(distanceFrom,p)) * p.GrowthRate() - p.NumShips();
                int distFromMe = distances.Distance(home, p);
                int distFromThem = distances.Distance(enemyHome, p);
                //Only consider if its closer to me than to them
                if (distFromMe <= distFromThem) {
                    int value = (distFromThem - distFromMe) * p.GrowthRate() - p.NumShips();
                    valueSorted.put(value, p);
                }
            }

            Iterator i = valueSorted.values().iterator();
            while (i.hasNext()) {
                Planet toAttack = (Planet)i.next();
                if ((home.NumShips() - 10) > toAttack.NumShips()) {
                    int sendingShips = toAttack.NumShips() + 1;
                    pw.IssueOrder(home, toAttack, sendingShips);
                    home.NumShips(home.NumShips() - sendingShips);
                }
            }

            //Well most of my forces are used up
            ++turnNum;
            return;
        }

        write("==TURN " + ++turnNum);

        if (pw.EnemyFleets().size() > 0) {
            for (Fleet en : pw.EnemyFleets()) {
                //Check if i've already countered this force
                if (!counteredFleets.contains(en)) {

                    //if target is owned = UNDER ATTACK
                    int NeedShips = en.NumShips();
                    TreeSet<Forces> forces = Forces.GetForces(pw, distances, en.DestinationPlanet(), en.TurnsRemaining());

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

                                pw.IssueOrder(f.Planet, en.DestinationPlanet(), takingShips);
                                pw.GetPlanet(f.Planet).NumShips(f.AvaliableForces - takingShips);
                                NeedShips -= takingShips;
                                SentShips += takingShips;
                            }
                        }
                    }

                    write("Sent: " + SentShips + " to " + en.DestinationPlanet() + ", wanted: " + NeedShips);

                    //TODO is this better or worse??
                    if (SentShips > en.NumShips()) {
                        counteredFleets.add(en);
                    }
                } else {
                    write("--Already Countered: " + en.DestinationPlanet());
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

        //Cleanup coutneredFleets
        counteredFleets.retainAll(pw.EnemyFleets());



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


    HashMap<Integer, TargetPlanet> targets = null;
    public void LoadTargetPlanets(PlanetWars pw){
        targets = new HashMap<Integer, TargetPlanet>(pw.Planets().size());
        for (Planet p : pw.Planets()){
            targets.put(p.PlanetID(), new TargetPlanet(p));
        }
        for (Fleet f : pw.Fleets()){
            targets.get(f.DestinationPlanet()).AddFleet(f);
        }
    }


//    //Attack Closest... no matter if i can beat it or not
//    //TODO make it attack the closest that
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
        if (Debugging) {
            write(logFile, msg);
        }
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
