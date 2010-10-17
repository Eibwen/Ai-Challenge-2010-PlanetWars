/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package MirrorBot;

/**
 *
 * @author gwalker
 */
public class Point {
    public Double x, y;

    Point(double X, double Y) {
        x=X;
        y=Y;
    }

    final int sig = 1000;

    @Override
    public int hashCode() {
        return (int) (Math.floor(x * sig) * 17 + Math.floor(y * sig));
    }

    @Override
    public boolean equals(Object obj) {
        if (obj == null) {
            return false;
        }
        if (getClass() != obj.getClass()) {
            return false;
        }
        final Point other = (Point) obj;
       
        if (Math.floor(x * sig) == Math.floor(other.x * sig)
            && Math.floor(y * sig) == Math.floor(other.y * sig)) {
            return true;
        }

        return false;
    }

    @Override
    public String toString() {
        return "(" + x + ", " + y + ")";
    }
}
