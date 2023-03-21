using System;
using System.Text;

namespace hashes;

public class GhostsTask : 
	IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
	IMagic
{
	private Vector vector = new Vector(0,0);
	private Segment segment;
	private Document document;
	private Robot robot = new Robot("Agent 47");
	private Cat cat = new Cat("Cat", "Breed", DateTime.Now);
	private byte[] array = { 228, 146, 15, 1, 8};

	public GhostsTask()
	{
        segment = new Segment(vector, vector);
		document = new Document("title", Encoding.Unicode, array);
    }

	public void DoMagic()
	{
		vector.Add(new Vector(146,146));
		array[1] = 232;
		cat.Rename("RenameCat");
		Robot.BatteryCapacity++;
	}

	Vector IFactory<Vector>.Create()
	{
		return vector;
	}

	Segment IFactory<Segment>.Create()
	{
		return segment;
	}

	Document IFactory<Document>.Create()
	{ 
		return document;
	}

	Cat IFactory<Cat>.Create()
	{
		return cat;
	}

	Robot IFactory<Robot>.Create() 
	{ 
		return robot;
	}
}