using System;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(PositionSaver))]
	public class ReplayMover : MonoBehaviour
	{
		private PositionSaver _save;

		private int _index;
		private PositionSaver.Data _prev;
		private float _duration;

		private void Start()
		{
			////todo comment: зачем нужны эти проверки?
			//// Ответ: Проверка на то, есть ли записи, а также, получается ли взять компонент типа PositionSaver. 
			//// Но не очень понимаю, ведь мы требуем этот компонент с помощью атрибута, как его может не быть?
			//// Но предположу, что из-за того, что он может быть выключен.
			if (!TryGetComponent(out _save) || _save.Records.Count == 0)
			{
				Debug.LogError("Records incorrect value", this);
				//todo comment: Для чего выключается этот компонент?
				// Ответ: Чтобы код этого компонента не работал, так как записей о позиции и времени нет.
				enabled = false;
			}
		}

		private void Update()
		{
			var curr = _save.Records[_index];
            //todo comment: Что проверяет это условие (с какой целью)? 
            // Ответ: Проверяет, что время смены позиции настало
            if (Time.time > curr.Time)
			{
				_prev = curr;
				_index++;
                //todo comment: Для чего нужна эта проверка?
                // Ответ: Чтобы узнать, когда мы прошли все точки
                if (_index >= _save.Records.Count)
				{
					enabled = false;
					Debug.Log($"<b>{name}</b> finished", this);
				}
			}
            //todo comment: Для чего производятся эти вычисления (как в дальнейшем они применяются)?
            // Ответ: Для того, чтобы узнать в процентном соотношение сколько прошло времени (от 0 до 1).
            var delta = (Time.time - _prev.Time) / (curr.Time - _prev.Time);
            //todo comment: Зачем нужна эта проверка?
            // Ответ: Не знаю, зачем нужно проверка, что delta not a number. Вроде это всегда float получается. Но, возможно, из-за var не всегда.
            if (float.IsNaN(delta)) delta = 0f;
            //todo comment: Опишите, что происходит в этой строчке так подробно, насколько это возможно
            // Ответ: Устанавливаем позицию объекту в зависимости от его процентного положения от начальной до конечной точки.
            transform.position = Vector3.Lerp(_prev.Position, curr.Position, delta);
		}
	}
}