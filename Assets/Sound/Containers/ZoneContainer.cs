using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ZoneContainer
{
    public static List<AudioClip> clipList = new();
    public static List<Collider2D> zones = new();
    public static List<Level> names = new();
    public static List<int> offsets = new();

    public static Level getColliderName(Collider2D collider) {
        int index = zones.IndexOf(collider);
        if(index < 0) {
            return Level.Blank;
        }
        return names[index];
    }

    public static Level getClipName(AudioClip clip) {
        int index = clipList.IndexOf(clip);
        if(index < 0) {
            return Level.Blank;
        }
        return names[index];
    }


    public static AudioClip GetClip(Level name) {
        int index = names.IndexOf(name);
        return clipList[index];
    }

    public static void SetOffset(Level name, int offset) {
        int index = names.IndexOf(name);
        offsets[index] = offset;
    }
    public static int GetOffset(Level name) {
        int index = names.IndexOf(name);
        return offsets[index];
    }

    public static void addZone(Collider2D collider) {
        zones.Add(collider);
    }
    public static void addName(Level name) {
        names.Add(name);
    }

    public static void addClip(AudioClip clip) {
        clipList.Add(clip);
    }

}
