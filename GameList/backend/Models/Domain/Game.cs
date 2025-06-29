namespace Models.Domain;

public class Game {
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public DateTime AdditionDate { get; set; } = DateTime.UtcNow;

    public Dictionary<Guid, bool> Owners { get; set; } = new();

    public bool IsExcluded { get; set; } = false;

    public void AddOwner(Person person) {
        if (Owners.Any(o => o.Key == person.Id)) {
            throw new InvalidOperationException("Person already exists.");
        }

        Owners.Add(person.Id, false);
    }

    public void RemoveOwner(Person person) {
        if (!Owners.Remove(person.Id)) {
            throw new InvalidOperationException("Person doesn't exist.");
        }
    }

    public void SetOwner(Person person, bool IsOwner) {
        if (Owners.ContainsKey(person.Id)) {
            Owners[person.Id] = IsOwner;
        }
        else {
            throw new InvalidOperationException("Person doesn't exist.");
        }
    }
}