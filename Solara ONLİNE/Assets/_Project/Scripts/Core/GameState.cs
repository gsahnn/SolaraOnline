// GameState.cs

// Bu dosya sadece oyunumuzun hangi durumda olabilece�ini tan�mlar.
// �rne�in: Ana Men�de miyiz, Oyunu mu oynuyoruz, yoksa oyun duraklat�ld� m�?
public enum GameState
{
    MainMenu,
    Loading,
    Gameplay,
    Paused
}