/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package CopyBot;

import java.io.FileWriter;
import java.util.ArrayList;
import java.util.List;
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

    List<Fleet> counteredFleets = new ArrayList<Fleet>();
    int turnNum = 0;

    public void DoTurn(PlanetWars pw) {

        write("==TURN " + ++turnNum);

        if (pw.EnemyFleets().size() > 0) {
            for (Fleet en : pw.EnemyFleets()) {
                //Check if i've already countered this force
                if (!counteredFleets.contains(en)) {

                    //if target is owned = UNDER ATTACK
                    int NeedShips = en.NumShips();
                    TreeSet<Forces> forces = Forces.GetForces(pw, en.DestinationPlanet(), en.TurnsRemaining());

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
