/* Includes ------------------------------------------------------------------*/
#include <stdio.h>
#include "emhal.h"

/* USER includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */

/* USER CODE END Includes */

/* Private typedef -----------------------------------------------------------*/
/* USER CODE BEGIN PTD */

/* USER CODE END PTD */

/* Private define ------------------------------------------------------------*/
/* USER CODE BEGIN PD */

/* USER CODE END PD */

/* Private macro -------------------------------------------------------------*/
/* USER CODE BEGIN PM */

/* USER CODE END PM */

/* Private variables ---------------------------------------------------------*/

/* USER CODE BEGIN PV */

/* USER CODE END PV */

/* Private function prototypes -----------------------------------------------*/

/* USER CODE BEGIN PFP */

/* USER CODE END PFP */

/* Private user code ---------------------------------------------------------*/
/* USER CODE BEGIN 0 */

/* USER CODE END 0 */

/* The application start entry point ---------------------------------------- */
{
/* Local variables */
	int ret = 0;
	int param = 0;

/* USER CODE BEGIN LV */

/* USER CODE END LV */

/* Init EMHAL */
	if (ret != 0)
	{
		printf("Init EMHAL failed.\n");
		return -1;
	}

/* Publish resource */

/* Subscribe resource */

/* USER CODE BEGIN */

/* USER CODE END*/

	return 0;
}

/* The application stop entry point ----------------------------------------- */
{
/* USER CODE BEGIN */

/* USER CODE END*/

/* Unpublish resource */

/* Unsubscribe resource */

	EMHAL_Uninit();
}


/* Subscribe resource handle  ----------------------------------------------- */
