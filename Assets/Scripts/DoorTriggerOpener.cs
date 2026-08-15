using UnityEngine;

public class DoorTriggerOpener : MonoBehaviour
{
	// Сюда перетаскиваем именно дочерний объект Door
	public GameObject doorObject;

	private void OnTriggerEnter(Collider other)
	{
		// Проверяем, что коснулся именно ключ
		if (other.CompareTag("Key"))
		{
			// Находим оригинальный скрипт от авторов ассета на объекте двери
			// В Unity он называется точно так же, как класс внутри него. Попробуем два частых варианта:

			// Вариант 1: Если скрипт называется Door
			var assetScript1 = doorObject.GetComponent("Door") as MonoBehaviour;
			if (assetScript1 != null)
			{
				// Пробуем вызвать функцию анимации через метод ассета
				assetScript1.Invoke("Open", 0f);
				assetScript1.Invoke("PlayAnimation", 0f);
			}

			// Вариант 2: Если встроенная анимация запускается через обычный Animator
			Animator anim = doorObject.GetComponent<Animator>();
			if (anim != null)
			{
				anim.Play("Door_Open");
				anim.SetBool("Open", true);
			}

			// Ключ исчезает (это у вас уже работает)
			Destroy(other.gameObject);
		}
	}
}

