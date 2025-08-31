// GameState.cs

// Bu dosya sadece oyunumuzun hangi durumda olabileceðini tanýmlar.
// Örneðin: Ana Menüde miyiz, Oyunu mu oynuyoruz, yoksa oyun duraklatýldý mý?
public enum GameState
{
    MainMenu,
    Loading,
    Gameplay,
    Paused
}