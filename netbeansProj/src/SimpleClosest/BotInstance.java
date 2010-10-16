/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package SimpleClosest;

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
public class BotInstance extends BotBase {
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


    //Attack Closest... no matter if i can beat it or not
    //TODO make it attack the closest that
    // problem with this is it just sends all the ships to the planet, then also sends while that fleet is traveling
    public void DoTurn(PlanetWars pw) {
        if (distances == null) {
            distances = new PlanetDistances(pw);
        }

        Boolean OnlyEnemyControlled = false;
        if (pw.Production(1) > (pw.Production(2) * 1.75)){
            //go in for the kill
            OnlyEnemyControlled = true;
        }

        //10 here beats Fanny, 20 beats Andy
        int AvaliableForcesDown = 10;
        int MinExtraOnPlanet = 20;
        int MinLeaveOnPlanet = 15;

        for (Planet p : pw.MyPlanets()) {
            if (p.BeingAttacked()) {

            }
            else {
                //Planet attack = Find.NearestPlanetNotOwned(pw, distances, p.PlanetID());
                //Planet attack = Find.NearestPlanetNotOwnedWeighted(pw, distances, p.PlanetID());
                Planet attack = Find.NearestPlanetNotOwnedCanTake(pw.Planets(), distances, p.PlanetID(), p.NumShips() - AvaliableForcesDown, OnlyEnemyControlled);
                if (attack != null && p.NumShips() > MinExtraOnPlanet) {
                    pw.IssueOrder(p.PlanetID(), attack.PlanetID(), p.NumShips() - MinLeaveOnPlanet);
                }
            }
        }
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
