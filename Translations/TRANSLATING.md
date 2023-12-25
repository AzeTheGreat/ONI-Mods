# Contributing to Translations

If you would like to help translate my mods, thank you!  You are in the right spot.  This document will walk you through the process.

## Obtaining Files

- Each translatable mod is a folder that can be found above.
- Translate a single mod:
  - Navigate into the folder of the mod you are interested in.  You should see a `_template.pot` file, and any existing `.po` translations.
  - [Download](#download-from-github) the `_template.pot` file.
  - Optional: [Download](#download-from-github) the relevant `.po` file. This may help if you are modifying an existing translation.
- Translate multiple mods:
  - Optional: [Download](#download-from-github) and unzip the `Translations.zip` file.  This allows downloading the entire contents of the `Translations` folder in one click for convenience.
  
## Translating

Start by reading  the [Official Translation Guide](https://forums.kleientertainment.com/forums/topic/74765-creatingusing-translation-files-updated-august-22nd-2017/).  This is not fully applicable, but it provides useful context.  The rest of the thread also has scattered information that may help when translating files.  
You may translate through software intended for translation, or with just a text editor.  If you are not confident, I recommend starting with Poedit.

##### Poedit:
- Download and install [Poedit](https://poedit.net/).
- Click "create new" and open the `_template.pot` file.
- Select the target language that you are translating to.
- Each line is a string that needs to be translated:
  - Click on a line to select it.
  - The bottom "source text" box shows the English string.  Do not modify this.
  - The "translation" box below shows the current translation.  Do your translating here.

##### Text Editor:
- You can translate with any text editor.  I recommend [Notepad++](https://notepad-plus-plus.org/) as a lightweight option.
- Each translation entry is a group of four lines:  
  `#.` *(Ignore) A comment of the internal ID.*  
  `msgctxt` "*(Ignore) The internal ID.*"  
  `msgid` "*The English source text*"  
  `msgstr` "*Where the new language text goes*"
- Simply translate the text in `msgid` and enter the translation in `msgstr`.

##### Finalizing
- Conditional: Set the font to be used (see the [Official Translation Guide](https://forums.kleientertainment.com/forums/topic/74765-creatingusing-translation-files-updated-august-22nd-2017/)).
- Optional: Append your name to the end of the file in this format: `#. Name`.
- Save the file as `[language_code].po`.

##### Considerations
- [Do not translate](https://forums.kleientertainment.com/forums/topic/74765-creatingusing-translation-files-updated-august-22nd-2017/?do=findComment&comment=871822) words inside curly braces.  The code looks for the key inside and replaces it with different text.
  - `English {object}` should be translated to `Español {object}`.
  - These can be rearranged if it suits localization: `English {object}` -> `{object} Español`
- [Do not translate](https://forums.kleientertainment.com/forums/topic/74765-creatingusing-translation-files-updated-august-22nd-2017/?do=findComment&comment=871822) words inside angle brackets.  These set the style of the text between.
  - `<style=\"stress\">English</style>` should be translated to `<style=\"stress\">Español</style>`
  - These may be removed or altered if it suits localization.

## Testing
Please test your translations to make sure everything works as intended before submitting them.
- Open the mod folder:
  - Navigate to your mods folder: `...\Documents\Klei\OxygenNotIncluded\mods\Steam`.
  - Search for the mod - its `.dll` will have the same name as the folder within `Translations`.
  - If using Windows Explorer: Right click on the search result and select "open file location".
- Add your `.po` file to the `Translations` folder in the mod.
  - If it does not have one, you will need to create it.
- Restart the game so that it can reload translations.

## Uploading
*Thank you for your help!*  
For your translation to be added, I will need the `.po` file. There are several ways you can get this file to me:
- Create a new issue on this repository (preferred).
- Create a PR.
- Contact me on Discord (Aze#0066).
- Link it in Steam Workshop comments.
  
#### Downloading from Github:
- Click into a file.
- Click the "download raw file" button at the top right of the file's text box.