package Common;

import Common.*;
import java.io.FileWriter;
import java.util.*;

/**
 *
 * @author mrflippy
 */
public abstract class BotBase {
    public void main(String[] args) {
        String line = "";
        String message = "";
        int c;
        try {
            while ((c = System.in.read()) >= 0) {
                switch (c) {
                    case '\n':
                        if (line.equals("go")) {
                            PlanetWars pw = new PlanetWars(message);

                            try {
                            this.DoTurn(pw);
                            } catch (Exception e) {
                                write("ERROR: " + e.getMessage());
                            }

                            pw.FinishTurn();
                            message = "";
                        } else {
                            message += line + "\n";
                        }
                        line = "";
                        break;
                    default:
                        line += (char) c;
                        break;
                }
            }
        } catch (Exception e) {
            // Owned.
            //write(e.toString());
        }
    }

    public abstract void DoTurn(PlanetWars pw);

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

