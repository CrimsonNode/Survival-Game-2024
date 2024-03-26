using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AtesEtme : MonoBehaviour
{ 
    public float jumpForce = 10f; // Zıplama kuvveti
    public float groundCheckDistance = 0.2f; // Yere temas kontrol mesafesi

    private Rigidbody rb;
    private bool isGrounded;
    RaycastHit hit;
    public GameObject MermiCikisNoktasi;
    public bool AtesEdebilir;
    float GunTimer;
    public float TaramaHizi;
    public ParticleSystem MuzzleFlash;
    AudioSource SesKaynak;
    public AudioClip AtesSesi;
    public float Menzil;
    public GameObject mermiEfekti;
    public float mermi;
    public float sarjor;
    public float tasinanmermi;
    float eklenenmermi;
    float reloadtimer;
    public Text mermisayac;
    // Start is called before the first frame update
    void Start()
    {
          rb = GetComponent<Rigidbody>();
        SesKaynak = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        // Zıplama
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        mermisayac.text=mermi+"/"+tasinanmermi;


        eklenenmermi=sarjor-mermi;
        if(eklenenmermi>tasinanmermi){
            eklenenmermi=tasinanmermi;
        }
        if(Input.GetKeyDown(KeyCode.R)&&eklenenmermi>0 && tasinanmermi > 0){
            if(Time.time > reloadtimer){
                StartCoroutine(Reload());
                reloadtimer=Time.time;
            }
        }
        
        if (Input.GetKey(KeyCode.Mouse0) && AtesEdebilir == true && Time.time > GunTimer && mermi > 0)
        {
            Fire();
            GunTimer = Time.time + TaramaHizi;
            if (Physics.Raycast(MermiCikisNoktasi.transform.position, MermiCikisNoktasi.transform.forward, out hit, Menzil))
            {
                MuzzleFlash.Play();
                SesKaynak.PlayOneShot(AtesSesi); // PlayOneShot kullanımı tavsiye edilir.
                Debug.Log(hit.transform.name);
                Quaternion muzzleRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                Instantiate(mermiEfekti, hit.point, muzzleRotation); // Ateş efekti yaratırken rotasyon kullanılmıyor.
            }
        }
    }

    void Fire()
    {
        if(mermi<=30){
            AtesEdebilir=false;
        }
        if(mermi>0){
            AtesEdebilir=true;
            mermi--;
        }
        
        if (Physics.Raycast(MermiCikisNoktasi.transform.position, MermiCikisNoktasi.transform.forward, out hit, Menzil))
        {
            MuzzleFlash.Play();
            SesKaynak.Play();
            SesKaynak.clip = AtesSesi;
            Debug.Log(hit.transform.name);
        }
    }
        IEnumerator Reload(){
            yield return new WaitForSeconds(1.2f);
            mermi=mermi+eklenenmermi;
            tasinanmermi=tasinanmermi-eklenenmermi;
        }


}