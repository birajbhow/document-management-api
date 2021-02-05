# document-management-api

Steps to run the application

1) Download the code and open the solution in visual studio 2019 (preferably)
2) Hit F5 or Cotinue button in visual studio.
3) It will open the browser with Swagger api spec page.
4) You can run following operations

  i) Upload:
    
    - Use the /api/document/upload endpoint to upload a document
    
  ii) List:
    
    - Use the /api/document/list endpoint to list all the uploaded documents
    
  iii) Delete:
    
    - Run the List operation to retrive the list of documents with id
    
    - Select a document id to delete
    
    - Run the /api/document/delete endpoint using this document id
    
    - Run the List operation to confirm that the desired document is deleted
    
  iv) Download:
    
    - Run the List operation to retrive the list of documents with id
    
    - Select a document id to download
    
    - Run the /api/document/download endpoint using this document id
 
