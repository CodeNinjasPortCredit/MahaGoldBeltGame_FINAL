using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu{
	public class UISettingsManager : MonoBehaviour {

		public enum Platform {Desktop, Mobile};
		public Platform platform;
		// toggle buttons
		[Header("MOBILE SETTINGS")]
		public GameObject mobileSFXtext;
		public GameObject mobileMusictext;
		public GameObject mobileShadowofftextLINE;
		public GameObject mobileShadowlowtextLINE;
		public GameObject mobileShadowhightextLINE;

		[Header("VIDEO SETTINGS")]
		public GameObject fullscreentext;
		public GameObject ambientocclusiontext;
		public GameObject shadowofftextLINE;
		public GameObject shadowlowtextLINE;
		public GameObject shadowhightextLINE;
		public GameObject aaofftextLINE;
		public GameObject aa2xtextLINE;
		public GameObject aa4xtextLINE;
		public GameObject aa8xtextLINE;
		public GameObject vsynctext;
		public GameObject motionblurtext;
		public GameObject texturelowtextLINE;
		public GameObject texturemedtextLINE;
		public GameObject texturehightextLINE;
		public GameObject cameraeffectstext;

		[Header("GAME SETTINGS")]
		public GameObject showhudtext;
		public GameObject tooltipstext;
		public GameObject difficultynormaltext;
		public GameObject difficultynormaltextLINE;
		public GameObject difficultyhardcoretext;
		public GameObject difficultyhardcoretextLINE;

		[Header("CONTROLS SETTINGS")]
		public GameObject invertmousetext;

		// sliders
		public GameObject musicSlider;
		public GameObject sensitivityXSlider;
		public GameObject sensitivityYSlider;
		public GameObject mouseSmoothSlider;

		private float sliderValue = 0.0f;
		private float sliderValueXSensitivity = 0.0f;
		private float sliderValueYSensitivity = 0.0f;
		private float sliderValueSmoothing = 0.0f;

		// Every reference above is an optional slot on this menu template. This
		// project uses the template for its start screen only and never built the
		// settings panel, so the slots are empty. Reading one directly throws and
		// aborts the rest of the method, so all access goes through these helpers.
		static void SetActiveSafe(GameObject target, bool state){
			if(target != null) target.SetActive(state);
		}

		static void SetLabel(GameObject target, string value){
			if(target == null) return;
			TMP_Text label = target.GetComponent<TMP_Text>();
			if(label != null) label.text = value;
		}

		static Slider SliderOf(GameObject target){
			return target == null ? null : target.GetComponent<Slider>();
		}

		public void  Start (){
			// check difficulty
			if(PlayerPrefs.GetInt("NormalDifficulty") == 1){
				SetActiveSafe(difficultynormaltextLINE, true);
				SetActiveSafe(difficultyhardcoretextLINE, false);
			}
			else
			{
				SetActiveSafe(difficultyhardcoretextLINE, true);
				SetActiveSafe(difficultynormaltextLINE, false);
			}

			// check slider values
			Slider music = SliderOf(musicSlider);
			if(music != null) music.value = PlayerPrefs.GetFloat("MusicVolume");

			Slider sensitivityX = SliderOf(sensitivityXSlider);
			if(sensitivityX != null) sensitivityX.value = PlayerPrefs.GetFloat("XSensitivity");

			Slider sensitivityY = SliderOf(sensitivityYSlider);
			if(sensitivityY != null) sensitivityY.value = PlayerPrefs.GetFloat("YSensitivity");

			Slider smoothing = SliderOf(mouseSmoothSlider);
			if(smoothing != null) smoothing.value = PlayerPrefs.GetFloat("MouseSmoothing");

			// check full screen
			if(Screen.fullScreen == true){
				SetLabel(fullscreentext, "on");
			}
			else if(Screen.fullScreen == false){
				SetLabel(fullscreentext, "off");
			}

			// check hud value
			if(PlayerPrefs.GetInt("ShowHUD")==0){
				SetLabel(showhudtext, "off");
			}
			else{
				SetLabel(showhudtext, "on");
			}

			// check tool tip value
			if(PlayerPrefs.GetInt("ToolTips")==0){
				SetLabel(tooltipstext, "off");
			}
			else{
				SetLabel(tooltipstext, "on");
			}

			// check shadow distance/enabled
			if(platform == Platform.Desktop){
				if(PlayerPrefs.GetInt("Shadows") == 0){
					QualitySettings.shadowCascades = 0;
					QualitySettings.shadowDistance = 0;
					SetActiveSafe(shadowofftextLINE, true);
					SetActiveSafe(shadowlowtextLINE, false);
					SetActiveSafe(shadowhightextLINE, false);
				}
				else if(PlayerPrefs.GetInt("Shadows") == 1){
					QualitySettings.shadowCascades = 2;
					QualitySettings.shadowDistance = 75;
					SetActiveSafe(shadowofftextLINE, false);
					SetActiveSafe(shadowlowtextLINE, true);
					SetActiveSafe(shadowhightextLINE, false);
				}
				else if(PlayerPrefs.GetInt("Shadows") == 2){
					QualitySettings.shadowCascades = 4;
					QualitySettings.shadowDistance = 500;
					SetActiveSafe(shadowofftextLINE, false);
					SetActiveSafe(shadowlowtextLINE, false);
					SetActiveSafe(shadowhightextLINE, true);
				}
			}else if(platform == Platform.Mobile){
				if(PlayerPrefs.GetInt("MobileShadows") == 0){
					QualitySettings.shadowCascades = 0;
					QualitySettings.shadowDistance = 0;
					SetActiveSafe(mobileShadowofftextLINE, true);
					SetActiveSafe(mobileShadowlowtextLINE, false);
					SetActiveSafe(mobileShadowhightextLINE, false);
				}
				else if(PlayerPrefs.GetInt("MobileShadows") == 1){
					QualitySettings.shadowCascades = 2;
					QualitySettings.shadowDistance = 75;
					SetActiveSafe(mobileShadowofftextLINE, false);
					SetActiveSafe(mobileShadowlowtextLINE, true);
					SetActiveSafe(mobileShadowhightextLINE, false);
				}
				else if(PlayerPrefs.GetInt("MobileShadows") == 2){
					QualitySettings.shadowCascades = 4;
					QualitySettings.shadowDistance = 100;
					SetActiveSafe(mobileShadowofftextLINE, false);
					SetActiveSafe(mobileShadowlowtextLINE, false);
					SetActiveSafe(mobileShadowhightextLINE, true);
				}
			}


			// check vsync
			if(QualitySettings.vSyncCount == 0){
				SetLabel(vsynctext, "off");
			}
			else if(QualitySettings.vSyncCount == 1){
				SetLabel(vsynctext, "on");
			}

			// check mouse inverse
			if(PlayerPrefs.GetInt("Inverted")==0){
				SetLabel(invertmousetext, "off");
			}
			else if(PlayerPrefs.GetInt("Inverted")==1){
				SetLabel(invertmousetext, "on");
			}

			// check motion blur
			if(PlayerPrefs.GetInt("MotionBlur")==0){
				SetLabel(motionblurtext, "off");
			}
			else if(PlayerPrefs.GetInt("MotionBlur")==1){
				SetLabel(motionblurtext, "on");
			}

			// check ambient occlusion
			if(PlayerPrefs.GetInt("AmbientOcclusion")==0){
				SetLabel(ambientocclusiontext, "off");
			}
			else if(PlayerPrefs.GetInt("AmbientOcclusion")==1){
				SetLabel(ambientocclusiontext, "on");
			}

			// check texture quality
			if(PlayerPrefs.GetInt("Textures") == 0){
				QualitySettings.globalTextureMipmapLimit = 2;
				SetActiveSafe(texturelowtextLINE, true);
				SetActiveSafe(texturemedtextLINE, false);
				SetActiveSafe(texturehightextLINE, false);
			}
			else if(PlayerPrefs.GetInt("Textures") == 1){
				QualitySettings.globalTextureMipmapLimit = 1;
				SetActiveSafe(texturelowtextLINE, false);
				SetActiveSafe(texturemedtextLINE, true);
				SetActiveSafe(texturehightextLINE, false);
			}
			else if(PlayerPrefs.GetInt("Textures") == 2){
				QualitySettings.globalTextureMipmapLimit = 0;
				SetActiveSafe(texturelowtextLINE, false);
				SetActiveSafe(texturemedtextLINE, false);
				SetActiveSafe(texturehightextLINE, true);
			}
		}

		public void Update (){
			//sliderValue = musicSlider.GetComponent<Slider>().value;
			Slider sensitivityX = SliderOf(sensitivityXSlider);
			if(sensitivityX != null) sliderValueXSensitivity = sensitivityX.value;

			Slider sensitivityY = SliderOf(sensitivityYSlider);
			if(sensitivityY != null) sliderValueYSensitivity = sensitivityY.value;

			Slider smoothing = SliderOf(mouseSmoothSlider);
			if(smoothing != null) sliderValueSmoothing = smoothing.value;
		}

		public void FullScreen (){
			Screen.fullScreen = !Screen.fullScreen;

			if(Screen.fullScreen == true){
				SetLabel(fullscreentext, "on");
			}
			else if(Screen.fullScreen == false){
				SetLabel(fullscreentext, "off");
			}
		}

		public void MusicSlider (){
			//PlayerPrefs.SetFloat("MusicVolume", sliderValue);
			Slider music = SliderOf(musicSlider);
			if(music != null) PlayerPrefs.SetFloat("MusicVolume", music.value);
		}

		public void SensitivityXSlider (){
			PlayerPrefs.SetFloat("XSensitivity", sliderValueXSensitivity);
		}

		public void SensitivityYSlider (){
			PlayerPrefs.SetFloat("YSensitivity", sliderValueYSensitivity);
		}

		public void SensitivitySmoothing (){
			PlayerPrefs.SetFloat("MouseSmoothing", sliderValueSmoothing);
			Debug.Log(PlayerPrefs.GetFloat("MouseSmoothing"));
		}

		// the playerprefs variable that is checked to enable hud while in game
		public void ShowHUD (){
			if(PlayerPrefs.GetInt("ShowHUD")==0){
				PlayerPrefs.SetInt("ShowHUD",1);
				SetLabel(showhudtext, "on");
			}
			else if(PlayerPrefs.GetInt("ShowHUD")==1){
				PlayerPrefs.SetInt("ShowHUD",0);
				SetLabel(showhudtext, "off");
			}
		}

		// the playerprefs variable that is checked to enable mobile sfx while in game
		public void MobileSFXMute (){
			if(PlayerPrefs.GetInt("Mobile_MuteSfx")==0){
				PlayerPrefs.SetInt("Mobile_MuteSfx",1);
				SetLabel(mobileSFXtext, "on");
			}
			else if(PlayerPrefs.GetInt("Mobile_MuteSfx")==1){
				PlayerPrefs.SetInt("Mobile_MuteSfx",0);
				SetLabel(mobileSFXtext, "off");
			}
		}

		public void MobileMusicMute (){
			if(PlayerPrefs.GetInt("Mobile_MuteMusic")==0){
				PlayerPrefs.SetInt("Mobile_MuteMusic",1);
				SetLabel(mobileMusictext, "on");
			}
			else if(PlayerPrefs.GetInt("Mobile_MuteMusic")==1){
				PlayerPrefs.SetInt("Mobile_MuteMusic",0);
				SetLabel(mobileMusictext, "off");
			}
		}

		// show tool tips like: 'How to Play' control pop ups
		public void ToolTips (){
			if(PlayerPrefs.GetInt("ToolTips")==0){
				PlayerPrefs.SetInt("ToolTips",1);
				SetLabel(tooltipstext, "on");
			}
			else if(PlayerPrefs.GetInt("ToolTips")==1){
				PlayerPrefs.SetInt("ToolTips",0);
				SetLabel(tooltipstext, "off");
			}
		}

		public void NormalDifficulty (){
			SetActiveSafe(difficultyhardcoretextLINE, false);
			SetActiveSafe(difficultynormaltextLINE, true);
			PlayerPrefs.SetInt("NormalDifficulty",1);
			PlayerPrefs.SetInt("HardCoreDifficulty",0);
		}

		public void HardcoreDifficulty (){
			SetActiveSafe(difficultyhardcoretextLINE, true);
			SetActiveSafe(difficultynormaltextLINE, false);
			PlayerPrefs.SetInt("NormalDifficulty",0);
			PlayerPrefs.SetInt("HardCoreDifficulty",1);
		}

		public void ShadowsOff (){
			PlayerPrefs.SetInt("Shadows",0);
			QualitySettings.shadowCascades = 0;
			QualitySettings.shadowDistance = 0;
			SetActiveSafe(shadowofftextLINE, true);
			SetActiveSafe(shadowlowtextLINE, false);
			SetActiveSafe(shadowhightextLINE, false);
		}

		public void ShadowsLow (){
			PlayerPrefs.SetInt("Shadows",1);
			QualitySettings.shadowCascades = 2;
			QualitySettings.shadowDistance = 75;
			SetActiveSafe(shadowofftextLINE, false);
			SetActiveSafe(shadowlowtextLINE, true);
			SetActiveSafe(shadowhightextLINE, false);
		}

		public void ShadowsHigh (){
			PlayerPrefs.SetInt("Shadows",2);
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowDistance = 500;
			SetActiveSafe(shadowofftextLINE, false);
			SetActiveSafe(shadowlowtextLINE, false);
			SetActiveSafe(shadowhightextLINE, true);
		}

		public void MobileShadowsOff (){
			PlayerPrefs.SetInt("MobileShadows",0);
			QualitySettings.shadowCascades = 0;
			QualitySettings.shadowDistance = 0;
			SetActiveSafe(mobileShadowofftextLINE, true);
			SetActiveSafe(mobileShadowlowtextLINE, false);
			SetActiveSafe(mobileShadowhightextLINE, false);
		}

		public void MobileShadowsLow (){
			PlayerPrefs.SetInt("MobileShadows",1);
			QualitySettings.shadowCascades = 2;
			QualitySettings.shadowDistance = 75;
			SetActiveSafe(mobileShadowofftextLINE, false);
			SetActiveSafe(mobileShadowlowtextLINE, true);
			SetActiveSafe(mobileShadowhightextLINE, false);
		}

		public void MobileShadowsHigh (){
			PlayerPrefs.SetInt("MobileShadows",2);
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowDistance = 500;
			SetActiveSafe(mobileShadowofftextLINE, false);
			SetActiveSafe(mobileShadowlowtextLINE, false);
			SetActiveSafe(mobileShadowhightextLINE, true);
		}

		public void vsync (){
			if(QualitySettings.vSyncCount == 0){
				QualitySettings.vSyncCount = 1;
				SetLabel(vsynctext, "on");
			}
			else if(QualitySettings.vSyncCount == 1){
				QualitySettings.vSyncCount = 0;
				SetLabel(vsynctext, "off");
			}
		}

		public void InvertMouse (){
			if(PlayerPrefs.GetInt("Inverted")==0){
				PlayerPrefs.SetInt("Inverted",1);
				SetLabel(invertmousetext, "on");
			}
			else if(PlayerPrefs.GetInt("Inverted")==1){
				PlayerPrefs.SetInt("Inverted",0);
				SetLabel(invertmousetext, "off");
			}
		}

		public void MotionBlur (){
			if(PlayerPrefs.GetInt("MotionBlur")==0){
				PlayerPrefs.SetInt("MotionBlur",1);
				SetLabel(motionblurtext, "on");
			}
			else if(PlayerPrefs.GetInt("MotionBlur")==1){
				PlayerPrefs.SetInt("MotionBlur",0);
				SetLabel(motionblurtext, "off");
			}
		}

		public void AmbientOcclusion (){
			if(PlayerPrefs.GetInt("AmbientOcclusion")==0){
				PlayerPrefs.SetInt("AmbientOcclusion",1);
				SetLabel(ambientocclusiontext, "on");
			}
			else if(PlayerPrefs.GetInt("AmbientOcclusion")==1){
				PlayerPrefs.SetInt("AmbientOcclusion",0);
				SetLabel(ambientocclusiontext, "off");
			}
		}

		public void CameraEffects (){
			if(PlayerPrefs.GetInt("CameraEffects")==0){
				PlayerPrefs.SetInt("CameraEffects",1);
				SetLabel(cameraeffectstext, "on");
			}
			else if(PlayerPrefs.GetInt("CameraEffects")==1){
				PlayerPrefs.SetInt("CameraEffects",0);
				SetLabel(cameraeffectstext, "off");
			}
		}

		public void TexturesLow (){
			PlayerPrefs.SetInt("Textures",0);
			QualitySettings.globalTextureMipmapLimit = 2;
			SetActiveSafe(texturelowtextLINE, true);
			SetActiveSafe(texturemedtextLINE, false);
			SetActiveSafe(texturehightextLINE, false);
		}

		public void TexturesMed (){
			PlayerPrefs.SetInt("Textures",1);
			QualitySettings.globalTextureMipmapLimit = 1;
			SetActiveSafe(texturelowtextLINE, false);
			SetActiveSafe(texturemedtextLINE, true);
			SetActiveSafe(texturehightextLINE, false);
		}

		public void TexturesHigh (){
			PlayerPrefs.SetInt("Textures",2);
			QualitySettings.globalTextureMipmapLimit = 0;
			SetActiveSafe(texturelowtextLINE, false);
			SetActiveSafe(texturemedtextLINE, false);
			SetActiveSafe(texturehightextLINE, true);
		}
	}
}
