using UnityEngine;
using System.Collections;

public interface IState
{
  	void OnEnter (string prevState);
	void OnExit(string nextState);
	void OnUpdate();
}
