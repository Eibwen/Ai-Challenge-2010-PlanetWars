package StarterBot;

import Common.*;
import java.util.*;

public class MyBot extends BotBase {
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

    public void DoTurn(PlanetWars pw) {
        //"if you're ahead then take it easy, take few risks, and maintain the status quo.
        // If you're behind, take lots of risk to try to change the status quo"
        int numFleets = 1;
        boolean attackMode = false;
        if (pw.NumShips(1) >= pw.NumShips(2)) {
            numFleets = 1;
        } else {
            numFleets = 2;
        }

        int planetCount = pw.MyPlanets().size();
        numFleets = numFleets * planetCount;

        if (pw.MyFleets().size() >= numFleets) {
            return;
        }

        for (int i = 0; i < planetCount; ++i) {

            // (2) Find my strongest planet.
            Planet source = null;
            double sourceScore = Double.MIN_VALUE;
            for (Planet p : pw.MyPlanets()) {
                double score = (double) p.NumShips() / (1 + p.GrowthRate());
                if (score > sourceScore) {
                    sourceScore = score;
                    source = p;
                }
            }
            // (3) Find the weakest enemy or neutral planet.
            Planet dest = null;
            double destScore = Double.MIN_VALUE;
            for (Planet p : pw.NotMyPlanets()) {
                double score = (double) (1 + p.GrowthRate()) / p.NumShips();
                if (score > destScore) {
                    destScore = score;
                    dest = p;
                }
            }
            // (4) Send half the ships from my strongest planet to the weakest
            // planet that I do not own.
            if (source != null && dest != null) {
                int numShips = source.NumShips() / 2;
                pw.IssueOrder(source, dest, numShips);
            }
        }
    }
}

