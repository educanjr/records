
using BallastLane.Domain.Entities;
using System.Text;

namespace BallastLane.ApplicationTests.Utils;

internal static class ValueGeneratorUtility
{
    public static string GenerateString(int length)
    {
        Random rnd = new();
        const string alphaNumericString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "0123456789" + "abcdefghijklmnopqrstuvxyz";

        StringBuilder sb = new(length);

        for (int i = 0; i < length; i++)
        {

            // generate a random number between
            // 0 to AlphaNumericString variable length
            int index = rnd.Next(alphaNumericString.Length);

            // add Character one by one in end of sb
            sb.Append(alphaNumericString[index]);
        }

        return sb.ToString();
    }

    public static User GenerateUser() => new(Guid.NewGuid(), "testc@email.com", "Firstname", "Last Name", "password");
    public static User GenerateUser(Guid id) => new(id, "testc@email.com", "Firstname", "Last Name", "password");
    public static Domain.Entities.Record GenerateRecord() => new(Guid.NewGuid(), GenerateUser(), "Title", "Description");
    public static Domain.Entities.Record GenerateRecord(Guid id) => new(id, GenerateUser(), "Title", "Description");
    public static Domain.Entities.Record GenerateRecord(Guid id, Guid creatorId) => new(id, GenerateUser(creatorId), "Title", "Description");
}
