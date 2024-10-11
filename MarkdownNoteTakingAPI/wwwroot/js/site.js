const uri = "Markdown";

document.getElementById("get-note").addEventListener("click", getMarkdownFile);
async function getMarkdownFile() {
  const markdownFile = document.getElementById("note-field");
  const fileName = document.getElementById("file-name-input").value;

  try {
    const response = await fetch(`${uri}/MarkdownFile/${fileName}`);
    const message = await response.json();
    console.log(message);
    markdownFile.innerHTML = message.markdownFile;
  } catch (error) {
    alert(error);
  }
}

document.getElementById("get-grammar").addEventListener("click", getGrammar);
async function getGrammar() {
  const grammarField = document.getElementById("grammar-field");
  const fileName = document.getElementById("file-name-grammar-input").value;
  grammarField.innerHTML = "";
  try {
    const response = await fetch(`${uri}/CheckGrammar/${fileName}`, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
    });
    const message = await response.json();
    console.log(message);
    grammarField.innerHTML = "";
    message.edits.forEach((element, i) => {
      let errorString = `${i + 1}) ${element.description}: \n\t '${
        element.sentence
      }' to '${element.replacement}'`;
      grammarField.innerHTML += errorString + "\n";
    });
  } catch (error) {
    alert(error);
  }
}

document
  .getElementById("upload-file")
  .addEventListener("click", uploadMarkdownFile);
async function uploadMarkdownFile() {
  const markdownFile = document.getElementById("myfile");
  const selectedFile = markdownFile.files[0];
  console.log(selectedFile);
  try{
    let data = new FormData()
    data.append('file', selectedFile)
    const upload = await fetch(`${uri}/MarkdownFile`, {
      method: "POST",
      body: data
    }); 
    const message = await upload.json();
    console.log(message);
  }
  catch(err)
  {
    console.log(err)
  }
}
