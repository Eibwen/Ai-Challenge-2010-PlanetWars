/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 *
 * @author gwalker
 */
public class MyBot {

    public static void main(String[] args) {
        Common.BotBase bot;
        //bot = new StarterBot.BotInstance();
        //bot = new SimpleClosest.BotInstance();
        //bot = new CopyBot.BotInstance();
        bot = new SpreadBot.BotInstance();

        bot.main(args);
    }
}
