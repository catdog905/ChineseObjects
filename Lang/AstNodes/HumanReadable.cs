namespace ChineseObjects.Lang;

public interface HumanReadable {
    /*
     * Return a list of strings that comprise a human-readable
     * representation of the node (with its children).
	 *
	 * When printed, each string should be one line.
     */
    public IList<string> GetRepr();
}