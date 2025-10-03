using UnityEngine;

namespace DefaultNamespace
{
	
	[RequireComponent(typeof(PositionSaver))]
	public class EditorMover : MonoBehaviour
	{
		private PositionSaver _save;
		private float _currentDelay;
		
		//todo comment: Что произойдёт, если _delay > _duration?
		// Ответ: Сохранение позиции и времени произойдет 1 раз
		[Range(0.2f, 1f)] private float _delay = 0.5f;
		[Min(0.2f)] private float _duration = 5f;

		private void Start()
		{
			if (_delay >= _duration)
			{
				_duration = _delay * 5;
			}

            //todo comment: Почему этот поиск производится здесь, а не в начале метода Update?
            // Ответ: Найти компонент достаточно 1 раз, он не предполагает меняться
            _save = GetComponent<PositionSaver>();
			_save.Records.Clear();
		}

		private void Update()
		{
			_duration -= Time.deltaTime;
			if (_duration <= 0f)
			{
				enabled = false;
				Debug.Log($"<b>{name}</b> finished", this);
				return;
			}

            //todo comment: Почему не написать (_delay -= Time.deltaTime;) по аналогии с полем _duration?
            // Ответ: Потому в дальнейшем задержка будет обновляться
            _currentDelay -= Time.deltaTime;
			if (_currentDelay <= 0f)
			{
				_currentDelay = _delay;
				_save.Records.Add(new PositionSaver.Data
				{
					Position = transform.position,
					//todo comment: Для чего сохраняется значение игрового времени?
					Time = Time.time,
				});
			}
		}
	}
}